using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface IBackgroundTasksService
	{
		Task<Guid> EnqueueAsync(ITaskStartParameters taskStartParameters);
		Task<BackgroundTaskStateModel> GetStateAsync(Guid queueGuid);
		Task<TaskResultModel> GetResultAsync(Guid queueGuid);
		Task WakeUpAsync(CancellationToken cancellationToken);
		Task<IEnumerable<BackgroundTaskStateModel>> GetAllStatesAsync();

		void OnQueueItemAdded(Action<ITaskStartParameters> callback);
		int PoolingDelayInMilliseconds { get; }
	}

	public class BackgroundTaskStateModel
	{
		public BackgroundTaskStateModel(QueueItemState state,
			DateTime? queueDate, DateTime? startDate,
			DateTime? endDate, int? numberOfTasksBefore,
			string taskName = null)
		{
			this.State = state;
			this.QueueDate = queueDate;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.NumberOfTasksBefore = numberOfTasksBefore;
			this.TaskName = taskName;
		}

		public string TaskName { get; }
		public QueueItemState State { get; }
		

		public DateTime? QueueDate { get; }
		public DateTime? StartDate { get; }
		public DateTime? EndDate { get; }

		public int? NumberOfTasksBefore { get; }
	}

	public enum QueueItemState
	{
		Waiting,
		Processing,
		Finished,
		Error,
		Cancelled,
		UnknownGuid
	}
}
