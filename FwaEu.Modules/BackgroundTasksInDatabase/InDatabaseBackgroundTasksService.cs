using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.BackgroundTasks;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasksInDatabase
{
	public class InDatabaseBackgroundTasksService : BackgroundTasksServiceBase
	{
		// NOTE: 30 seconds not to bother the database too much
		public override int PoolingDelayInMilliseconds => 30 * 1000; //TODO: pooling delay from config https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/5364

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly IServiceProvider _serviceProvider;
		private readonly AsyncLocalScopedServiceProviderAccessor _localScopeProviderAccessor;

		public InDatabaseBackgroundTasksService(
			ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory,
			IEnumerable<IBackgroundTaskFactory> backgroundTaskFactories,
			IEnumerable<IScheduledBackgroundTaskFactory> scheduledBackgroundTaskFactories,
			IServiceProvider serviceProvider,
			ICurrentDateTime currentDateTime,
			AsyncLocalScopedServiceProviderAccessor localScopeProviderAccessor) : base(backgroundTaskFactories,
				scheduledBackgroundTaskFactories, serviceProvider, currentDateTime)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._localScopeProviderAccessor = localScopeProviderAccessor
				?? throw new ArgumentNullException(nameof(localScopeProviderAccessor));
		}

		protected async Task<T> ExecuteInSessionAsync<T>(
			Func<IStatefulSessionAdapter, BackgroundTaskEntityRepository, Task<T>> actionAsync)
		{
			using (_localScopeProviderAccessor.BeginScope(_serviceProvider))
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<BackgroundTaskEntityRepository>(session);

				var result = await actionAsync(session, repository);
				return result;
			}
		}

		protected Task ExecuteAsync(ITaskExecution taskExecution,
			Func<BackgroundTaskEntity, Task> actionAsync)
		{
			return this.ExecuteAsync(
				async repository => await repository.GetAsync(taskExecution.Guid),
				actionAsync);
		}

		protected async Task ExecuteAsync(
			Func<BackgroundTaskEntityRepository, Task<BackgroundTaskEntity>> getEntityAsync,
			Func<BackgroundTaskEntity, Task> actionAsync)
		{
			await this.ExecuteInSessionAsync(async (session, repository) =>
			{
				var entity = await getEntityAsync(repository);

				if (actionAsync != null)
				{
					await actionAsync(entity);
				}

				await repository.SaveOrUpdateAsync(entity);
				await session.FlushAsync();

				return default(object); // HACK: Because ExecuteInSessionAsync require an actionAsync which return Task<T>
			});
		}

		public override async Task<TaskResultModel> GetResultAsync(Guid queueGuid)
		{
			return await this.ExecuteInSessionAsync(async (session, repository) =>
			{
				var entity = await repository.GetAsync(queueGuid);
				return new TaskResultModel(entity.ResultData, entity.ErrorMessage);
			});
		}

		protected async Task<ITaskExecution> FindExecutingAsync(Guid queueGuid)
		{
			using (await this.Lock.LockAsync())
			{
				var executing = this.GetExecutingUnsafe();
				return executing.FirstOrDefault(t => t.Guid == queueGuid);
			}
		}

		public override async Task<BackgroundTaskStateModel> GetStateAsync(Guid queueGuid)
		{
			return await this.ExecuteInSessionAsync(async (session, repository) =>
			{
				var entity = await repository.Query()
					.FirstOrDefaultAsync(bt => bt.QueueGuid == queueGuid);

				if (entity == null)
				{
					return new BackgroundTaskStateModel(QueueItemState.UnknownGuid, null, null, null, null);
				}

				var executing = await this.FindExecutingAsync(queueGuid);

				var state = executing == null
					? entity.EndedOn != null
						? entity.ErrorMessage == null
							? entity.WasCancelled
								? QueueItemState.Cancelled
								: QueueItemState.Finished
							: QueueItemState.Error
						: QueueItemState.Waiting
					: QueueItemState.Processing;

				var numberOfTasksBefore = default(int?);

				if (state == QueueItemState.Waiting)
				{
					numberOfTasksBefore = await repository.Query()
						.Where(bt => bt.StartedOn == null && bt.QueueDate < entity.QueueDate)
						.CountAsync();
				}

				return new BackgroundTaskStateModel(state,
					entity.QueueDate, entity.StartedOn,
					entity.EndedOn, numberOfTasksBefore, entity.StartParameters.TaskName);
			});
		}

		protected override Task EnqueueItemAsync(
			ITaskStartParameters taskStartParameters,
			DateTime queueDate, Guid guid)
		{
			return this.ExecuteAsync(
				_ => Task.FromResult(new BackgroundTaskEntity
				(
					guid,
					TaskStartParametersComponent.From(taskStartParameters),
					queueDate)
				),
				null);
		}

		protected override async Task<ITaskExecution[]> GetNextTaskExecutionsAsync(int count)
		{
			return await this.ExecuteInSessionAsync(async (session, repository) =>
			{
				var tasks = await repository.Query()
					.Where(bt => bt.StartedOn == null)
					.OrderBy(bt => bt.QueueDate)
					.Take(count)
					.Select(bt => new TaskExecution(
						bt.QueueGuid,
						bt.QueueDate,
						bt.StartParameters))
					.ToListAsync();

				return tasks.ToArray();
			});
		}

		private class TaskExecution : ITaskExecution
		{
			public TaskExecution(Guid guid, DateTime queueDate, ITaskStartParameters startParameters)
			{
				this.Guid = guid;
				this.QueueDate = queueDate;

				this.StartParameters = startParameters
					?? throw new ArgumentNullException(nameof(startParameters));
			}

			public Guid Guid { get; }
			public DateTime QueueDate { get; }
			public ITaskStartParameters StartParameters { get; }
		}

		private static void SetEndedOn(BackgroundTaskEntity entity, DateTime date)
		{
			entity.EndedOn = date;
		}

		protected override Task SetCancelledAsync(
			ITaskExecution taskExecution, DateTime date)
		{
			return this.ExecuteAsync(taskExecution,
				entity =>
				{
					entity.WasCancelled = true;
					SetEndedOn(entity, date);

					return Task.CompletedTask;
				});
		}

		protected override Task SetExceptionAsync(
			ITaskExecution taskExecution, DateTime date, Exception exception)
		{
			return this.ExecuteAsync(taskExecution,
				entity =>
				{
					entity.ErrorMessage = exception.Message;
					SetEndedOn(entity, date);

					return Task.CompletedTask;
				});
		}

		protected override Task SetResultAsync(
			ITaskExecution taskExecution, DateTime date, string result)
		{
			return this.ExecuteAsync(taskExecution,
				entity =>
				{
					entity.ResultData = result;
					SetEndedOn(entity, date);

					return Task.CompletedTask;
				});
		}

		protected override Task SetStartedAsync(ITaskExecution taskExecution, DateTime date)
		{
			return this.ExecuteAsync(taskExecution,
				entity =>
				{
					entity.StartedOn = date;
					return Task.CompletedTask;
				});
		}

		public override async Task<IEnumerable<BackgroundTaskStateModel>> GetAllStatesAsync()
		{
			return await this.ExecuteInSessionAsync(async (session, repository) =>
			{
				var executings = this.GetExecutingUnsafe();

				return (await repository.Query()
					.Select(e => new
					{
						e.QueueGuid,
						e.QueueDate,
						e.StartedOn,
						e.EndedOn,
						e.ErrorMessage,
						e.WasCancelled,
						NumberOfTasksBefore = repository.Query().Where(bt => bt.StartedOn == null && bt.QueueDate < e.QueueDate).Count(),
						e.StartParameters.TaskName,
					}).ToListAsync())
					.Select(e =>
					{
						var state = executings.FirstOrDefault(t => t.Guid == e.QueueGuid) == null
							? e.EndedOn != null
								? e.ErrorMessage == null
									? e.WasCancelled
										? QueueItemState.Cancelled
										: QueueItemState.Finished
									: QueueItemState.Error
								: QueueItemState.Waiting
							: QueueItemState.Processing;
						return new BackgroundTaskStateModel(state, e.QueueDate, e.StartedOn, e.EndedOn,
							state == QueueItemState.Waiting ? e.NumberOfTasksBefore : null, e.TaskName);
					});
			});
		}
	}
}
