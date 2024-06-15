using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Threading;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface ITaskExecution
	{
		Guid Guid { get; }
		ITaskStartParameters StartParameters { get; }
		DateTime QueueDate { get; }

	}

	public interface ITaskStateInfo
	{
		DateTime? StartedOn { get; set; }
		DateTime? EndedOn { get; set; }
	}

	public abstract class BackgroundTasksServiceBase : IBackgroundTasksService
	{
		private const int MaximumParallelism = 5; //TODO: from config https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/5364

		protected readonly AsyncLock Lock = new AsyncLock();
		private readonly List<ITaskExecution> _executing = new List<ITaskExecution>(MaximumParallelism);

		/// <summary>
		/// Should be called only using the Lock
		/// </summary>
		protected ITaskExecution[] GetExecutingUnsafe()
		{
			return this._executing.ToArray();
		}

		private readonly IBackgroundTaskFactory[] _backgroundTaskFactories;
		private readonly IScheduledBackgroundTaskFactory[] _scheduledBackgroundTaskFactories;

		private readonly IServiceProvider _serviceProvider;
		private readonly ICurrentDateTime _currentDateTime;
		private Action<ITaskStartParameters> _onQueueItemAddedCallback;

		public abstract int PoolingDelayInMilliseconds { get; }

		protected BackgroundTasksServiceBase(
			IEnumerable<IBackgroundTaskFactory> backgroundTaskFactories,
			IEnumerable<IScheduledBackgroundTaskFactory> scheduledBackgroundTaskFactories,
			IServiceProvider serviceProvider,
			ICurrentDateTime currentDateTime)
		{
			this._backgroundTaskFactories = backgroundTaskFactories.ToArray();
			this._scheduledBackgroundTaskFactories = scheduledBackgroundTaskFactories.ToArray();

			this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this._currentDateTime = currentDateTime ?? throw new ArgumentNullException(nameof(currentDateTime));
		}

		protected abstract Task EnqueueItemAsync(
			ITaskStartParameters taskStartParameters,
			DateTime queueDate, Guid guid);

		public async Task<Guid> EnqueueAsync(ITaskStartParameters taskStartParameters)
		{
			var guid = Guid.NewGuid();

			using (await this.Lock.LockAsync())
			{
				await this.EnqueueItemAsync(taskStartParameters, this._currentDateTime.Now, guid);
			}

			this._onQueueItemAddedCallback?.Invoke(taskStartParameters);

			return guid;
		}

		public void OnQueueItemAdded(Action<ITaskStartParameters> callback)
		{
			this._onQueueItemAddedCallback = callback;
		}

		protected abstract Task SetStartedAsync(ITaskExecution taskExecution, DateTime date);
		protected abstract Task SetResultAsync(ITaskExecution taskExecution, DateTime date, string result);
		protected abstract Task SetExceptionAsync(ITaskExecution taskExecution, DateTime date, Exception exception);
		protected abstract Task SetCancelledAsync(ITaskExecution taskExecution, DateTime date);
		protected abstract Task<ITaskExecution[]> GetNextTaskExecutionsAsync(int count);

		private async Task ExecuteFullQueueAsync(CancellationToken cancellationToken)
		{
			var executing = this._executing;

			cancellationToken.ThrowIfCancellationRequested();
			cancellationToken.Register(async () =>
			{
				var now = this._currentDateTime.Now;

				foreach (var taskExecution in executing)
				{
					await this.SetCancelledAsync(taskExecution, now);
				}

				executing.Clear();
			});

			var loopAgain = true;
			while (loopAgain || executing.Count > 0)
			{
				var queue = default(Queue<ITaskExecution>);

				using (await this.Lock.LockAsync())
				{
					if (executing.Count < MaximumParallelism)
					{
						var next = await this.GetNextTaskExecutionsAsync(MaximumParallelism - executing.Count);
						loopAgain = next.Length > 0; //NOTE: Maybe there is more items to execute

						queue = new Queue<ITaskExecution>(next);
					}

					while (queue != null && queue.Count > 0 && executing.Count < MaximumParallelism)
					{
						var taskExecution = queue.Dequeue();
						executing.Add(taskExecution);

						await this.SetStartedAsync(taskExecution, this._currentDateTime.Now);

						_ = Task.Factory.StartNew(async delegate
						{
							try
							{
								var result = await this.ExecuteTaskAsync(taskExecution, cancellationToken);
								await this.SetResultAsync(taskExecution, this._currentDateTime.Now, result);
							}
							catch (Exception ex)
							{
								var error = new BackgroundTaskExecutionException(
									$"Error during execution of task '{taskExecution.StartParameters.TaskName}'" +
									$" with id '{taskExecution.Guid}'.", ex);

								try
								{
									await this.SetExceptionAsync(taskExecution, this._currentDateTime.Now, ex);
								}
								catch (Exception ex2)
								{
									throw new BackgroundTaskExecutionException(
										$"Error during execution of task '{taskExecution.StartParameters.TaskName}'" +
										$" then error while saving error message, for task" +
										$" with id '{taskExecution.Guid}'.", ex2);
								}

								throw error;
							}
							finally
							{
								using (await this.Lock.LockAsync())
								{
									if (executing.Contains(taskExecution)) // NOTE: Case when the executing.Clear() (in the cancellationToken.Register() previously) was called before
									{
										executing.Remove(taskExecution);
									}
								}
							}
						},
						CancellationToken.None, //NOTE: Do not use cancellationToken otherwise the try/finally will not be used
						TaskCreationOptions.LongRunning,
						TaskScheduler.Current);
					}
				}

				await Task.Delay(200); //NOTE: No need for configuration
			}
		}

		public async Task WakeUpAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			await this.ExecuteFullQueueAsync(cancellationToken);
			await this.EnqueueScheduledTasksAsync(cancellationToken);
		}

		private async Task EnqueueScheduledTasksAsync(CancellationToken cancellationToken)
		{
			var toQueue = new List<ITaskStartParameters>(this._scheduledBackgroundTaskFactories.Length);

			foreach (var factory in this._scheduledBackgroundTaskFactories)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var taskStartParameters = await factory.GetScheduledTaskStartParametersAsync(this._serviceProvider);
				if (taskStartParameters != null)
				{
					toQueue.Add(taskStartParameters);
				}
			}

			foreach (var taskStartParameters in toQueue)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await this.EnqueueAsync(taskStartParameters);
			}
		}

		private async Task<string> ExecuteTaskAsync(
			ITaskExecution taskExecution,
			CancellationToken cancellationToken)
		{
			using (var taskLocalScope = _serviceProvider.CreateScope())
			{
				// Has to run a Task so that AsyncLocalScopedServiceProviderAccessor._scopedServiceProviderCurrent is set in the children tasks.
				return await Task.Run(async () =>
				{
					using (taskLocalScope.ServiceProvider.GetService<AsyncLocalScopedServiceProviderAccessor>().BeginScope(taskLocalScope.ServiceProvider))
					{
						cancellationToken.ThrowIfCancellationRequested();

						var task = this._backgroundTaskFactories
							.Select(factory => factory.CreateTask(taskExecution.StartParameters.TaskName, taskLocalScope.ServiceProvider))
							.FirstOrDefault(t => t != null);

						if (task == null)
						{
							throw new BackgroundTaskFactoryNotFoundException(
								$"No factory found to create task with name '{taskExecution.StartParameters.TaskName}', are you sharing your database?");
						}
						return await task.ExecuteAsync(taskExecution.StartParameters, cancellationToken);
					}
				});
			}
		}

		public abstract Task<BackgroundTaskStateModel> GetStateAsync(Guid queueGuid);
		public abstract Task<TaskResultModel> GetResultAsync(Guid queueGuid);

		public abstract Task<IEnumerable<BackgroundTaskStateModel>> GetAllStatesAsync();
	}
}
