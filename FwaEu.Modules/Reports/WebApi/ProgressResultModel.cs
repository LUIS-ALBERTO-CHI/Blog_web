using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ProgressResultModel
	{
		public ProgressResultModel(AsyncReportQueueState state,
			DateTime? queueDate, DateTime? startDate,
			DateTime? endDate, int? numberOfTasksBefore,
			string reportCacheStoreKey)
		{
			this.State = state;

			this.QueueDate = queueDate;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.NumberOfTasksBefore = numberOfTasksBefore;

			this.ReportCacheStoreKey = reportCacheStoreKey;
		}

		public AsyncReportQueueState State { get; }

		public DateTime? QueueDate { get; }
		public DateTime? StartDate { get; }
		public DateTime? EndDate { get; }
		public int? NumberOfTasksBefore { get; }

		public string ReportCacheStoreKey { get; }
	}

	public class ProgressErrorModel
	{
		public ProgressErrorModel(string errorMessage)
		{
			this.ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
		}

		public string ErrorMessage { get; }
	}
}
