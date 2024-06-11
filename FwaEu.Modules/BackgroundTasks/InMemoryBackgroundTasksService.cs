using FwaEu.Fwamework.Temporal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public class InMemoryTaskExecution : ITaskExecution
	{
		public InMemoryTaskExecution(
			ITaskStartParameters startParameters,
			DateTime queueDate, Guid guid)
		{
			this.StartParameters = startParameters
				?? throw new ArgumentNullException(nameof(startParameters));

			this.QueueDate = queueDate;

			this.Guid = guid;
		}

		public Guid Guid { get; }
		public DateTime QueueDate { get; }

		public ITaskStartParameters StartParameters { get; }

		public string ResultData { get; set; }
		public string ErrorMessage { get; set; }

		public bool WasCancelled { get; set; }

		public DateTime? StartedOn { get; set; }
		public DateTime? EndedOn { get; set; }
	}


	public class InMemoryBackgroundTasksService : BackgroundTasksServiceBase
	{
		//NOTE: 5 seconds because in memory, it cost very few to check the queue
		public override int PoolingDelayInMilliseconds => 5 * 1000; //TODO: pooling delay from config https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/5364

		private readonly Queue<InMemoryTaskExecution> _queue
			= new Queue<InMemoryTaskExecution>();

		private readonly Dictionary<Guid, InMemoryTaskExecution> _results
			= new Dictionary<Guid, InMemoryTaskExecution>();

		public InMemoryBackgroundTasksService(IEnumerable<IBackgroundTaskFactory> backgroundTaskFactories,
			IEnumerable<IScheduledBackgroundTaskFactory> scheduledBackgroundTaskFactories,
			IServiceProvider serviceProvider, ICurrentDateTime currentDateTime)
			: base(backgroundTaskFactories, scheduledBackgroundTaskFactories, serviceProvider, currentDateTime)
		{
		}

		protected override Task EnqueueItemAsync(ITaskStartParameters taskStartParameters, DateTime queueDate, Guid guid)
		{
			var queueItem = new InMemoryTaskExecution(taskStartParameters, queueDate, guid);
			this._queue.Enqueue(queueItem);

			return Task.CompletedTask;
		}

		private void SetTaskFinished(InMemoryTaskExecution inMemoryTaskExecution, DateTime date)
		{
			inMemoryTaskExecution.EndedOn = date;
			this._results.Add(inMemoryTaskExecution.Guid, inMemoryTaskExecution);
		}

		protected override Task SetStartedAsync(ITaskExecution taskExecution, DateTime date)
		{
			var inMemoryTaskExecution = (InMemoryTaskExecution)taskExecution;
			inMemoryTaskExecution.StartedOn = date;

			return Task.CompletedTask;
		}

		protected override Task SetResultAsync(ITaskExecution taskExecution, DateTime date, string result)
		{
			var inMemoryTaskExecution = (InMemoryTaskExecution)taskExecution;
			inMemoryTaskExecution.ResultData = result;
			
			this.SetTaskFinished(inMemoryTaskExecution, date);

			return Task.CompletedTask;
		}

		protected override Task SetCancelledAsync(ITaskExecution taskExecution, DateTime date)
		{
			var inMemoryTaskExecution = (InMemoryTaskExecution)taskExecution;
			inMemoryTaskExecution.WasCancelled = true;

			this.SetTaskFinished(inMemoryTaskExecution, date);

			return Task.CompletedTask;
		}

		protected override Task SetExceptionAsync(ITaskExecution taskExecution, DateTime date, Exception exception)
		{
			var inMemoryTaskExecution = (InMemoryTaskExecution)taskExecution;
			inMemoryTaskExecution.ErrorMessage = exception.Message;

			this.SetTaskFinished(inMemoryTaskExecution, date);

			return Task.CompletedTask;
		}

		public override async Task<BackgroundTaskStateModel> GetStateAsync(Guid queueGuid)
		{
			var taskExecution = default(InMemoryTaskExecution);
			var state = QueueItemState.UnknownGuid;
			var numberOfTasksBefore = default(int?);

			using (await this.Lock.LockAsync())
			{
				if ((taskExecution = this._queue.FirstOrDefault(qi => qi.Guid == queueGuid)) != null)
				{
					state = QueueItemState.Waiting;

					var items = this._queue.ToArray();
					numberOfTasksBefore = 0;

					foreach (var item in items)
					{
						if (item == taskExecution)
						{
							break;
						}

						numberOfTasksBefore++;
					}
				}
				else
				{
					if (this._results.ContainsKey(queueGuid))
					{
						taskExecution = this._results[queueGuid];

						if (taskExecution.WasCancelled)
						{
							state = QueueItemState.Cancelled;
						}
						else if (taskExecution.ErrorMessage != null)
						{
							state = QueueItemState.Error;
						}
						else
						{
							state = QueueItemState.Finished;
						}
					}
					else if ((taskExecution = (InMemoryTaskExecution)this.GetExecutingUnsafe()
						.FirstOrDefault(te => te.Guid == queueGuid)) != null)
					{
						state = QueueItemState.Processing;
					}
				}
			}

			return new BackgroundTaskStateModel(state,
				taskExecution?.QueueDate, taskExecution?.StartedOn,
				taskExecution?.EndedOn, numberOfTasksBefore, taskExecution?.StartParameters.TaskName);
		}

		public override Task<TaskResultModel> GetResultAsync(Guid queueGuid)
		{
			//TODO: Lifetime https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/5176

			if (this._results.ContainsKey(queueGuid))
			{
				var result = this._results[queueGuid];
				return Task.FromResult(new TaskResultModel(result.ResultData, result.ErrorMessage));
			}

			return Task.FromResult(default(TaskResultModel));
		}

		protected override Task<ITaskExecution[]> GetNextTaskExecutionsAsync(int count)
		{
			var list = new List<ITaskExecution>(count);
			
			for (var i = 0; i < count; i++)
			{
				if (this._queue.Count == 0)
				{
					break;
				}

				list.Add(this._queue.Dequeue());
			}

			return Task.FromResult(list.ToArray());
		}

		public override Task<IEnumerable<BackgroundTaskStateModel>> GetAllStatesAsync()
		{
			var backgroundTaskStates = new List<BackgroundTaskStateModel>();

			backgroundTaskStates = backgroundTaskStates.Concat(
				this._queue.Select((q, index) => new BackgroundTaskStateModel(QueueItemState.Waiting, 
					q.QueueDate, q.StartedOn, q.EndedOn, index - 1, q.StartParameters.TaskName)
				)
			).Concat(
				this._results.Select((r, index) =>
				{
					var state = QueueItemState.Finished;
					
					if (r.Value.WasCancelled)
					{
						state = QueueItemState.Cancelled;
					}
					else if (r.Value.ErrorMessage != null)
					{
						state = QueueItemState.Error;
					}
					return new BackgroundTaskStateModel(state,
						r.Value.QueueDate, r.Value.StartedOn, r.Value.EndedOn, index - 1, r.Value.StartParameters.TaskName);
				})
			).Concat(
				this.GetExecutingUnsafe().Cast<InMemoryTaskExecution>()
					.Select((q, index) => new BackgroundTaskStateModel(QueueItemState.Processing,
						q.QueueDate, q.StartedOn, q.EndedOn, index - 1, q.StartParameters.TaskName)
				)
			).ToList();
			return Task.FromResult<IEnumerable<BackgroundTaskStateModel>>(backgroundTaskStates);
		}
	}
}
