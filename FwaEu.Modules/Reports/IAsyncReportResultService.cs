using FwaEu.Fwamework.Data;
using FwaEu.Modules.BackgroundTasks;
using System;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IAsyncReportResultService
	{
		Task<AsyncReportQueueStateResultModel> GetStateAsync(Guid queueGuid);
	}

	public class AsyncReportQueueStateResultModel
	{
		public AsyncReportQueueStateResultModel(
			AsyncReportQueueState state,
			DateTime? queueDate, DateTime? startDate,
			DateTime? endDate, int? numberOfTasksBefore,
			string reportCacheStoreKey,
			string errorMessage)
		{
			this.State = state;

			this.QueueDate = queueDate;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.NumberOfTasksBefore = numberOfTasksBefore;

			this.ReportCacheStoreKey = reportCacheStoreKey;
			this.ErrorMessage = errorMessage;
		}

		public AsyncReportQueueState State { get; }

		public DateTime? QueueDate { get; }
		public DateTime? StartDate { get; }
		public DateTime? EndDate { get; }
		public int? NumberOfTasksBefore { get; }

		public string ReportCacheStoreKey { get; }
		public string ErrorMessage { get; }
	}

	public enum AsyncReportQueueState
	{
		Waiting,
		Processing,
		Finished
	}

	public class DefaultAsyncReportResultService : IAsyncReportResultService
	{
		private readonly IBackgroundTasksService _backgroundTasksService;

		public DefaultAsyncReportResultService(IBackgroundTasksService backgroundTasksService)
		{
			this._backgroundTasksService = backgroundTasksService
				?? throw new ArgumentNullException(nameof(backgroundTasksService));
		}

		public async Task<AsyncReportQueueStateResultModel> GetStateAsync(Guid queueGuid)
		{
			var stateModel = await this._backgroundTasksService.GetStateAsync(queueGuid);

			switch (stateModel.State)
			{
				case QueueItemState.UnknownGuid:
					throw new NotFoundException($"Unknown guid '{queueGuid}' in queue.");

				case QueueItemState.Waiting:
				case QueueItemState.Processing:
					return new AsyncReportQueueStateResultModel(stateModel.State == QueueItemState.Waiting
						? AsyncReportQueueState.Waiting : AsyncReportQueueState.Processing,
						stateModel.QueueDate, stateModel.StartDate, stateModel.EndDate, stateModel.NumberOfTasksBefore,
						null, null);

				case QueueItemState.Finished:
				case QueueItemState.Cancelled:
				case QueueItemState.Error:

					var result = stateModel.State == QueueItemState.Finished || stateModel.State == QueueItemState.Error
						? await this._backgroundTasksService.GetResultAsync(queueGuid)
						: null;

					return new AsyncReportQueueStateResultModel(
						AsyncReportQueueState.Finished, // NOTE: No needs for a "Cancelled" state in the report controller, data will be null and client will do a new generation request
						stateModel.QueueDate, stateModel.StartDate, stateModel.EndDate, stateModel.NumberOfTasksBefore,
						result?.Data, result?.ErrorMessage);
			}

			throw new NotSupportedException();
		}
	}
}