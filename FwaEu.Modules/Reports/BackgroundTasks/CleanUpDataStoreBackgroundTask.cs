using FwaEu.Modules.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.BackgroundTasks
{
	public class CleanUpDataStoreBackgroundTask : IBackgroundTask
	{
		public const string TaskName = "CleanUpDataStore";

		private readonly IAsyncReportDataStoreService _asyncReportDataStoreService;

		public CleanUpDataStoreBackgroundTask(IAsyncReportDataStoreService asyncReportDataStoreService)
		{
			this._asyncReportDataStoreService = asyncReportDataStoreService
				?? throw new ArgumentNullException(nameof(asyncReportDataStoreService));
		}

		public async Task<string> ExecuteAsync(ITaskStartParameters taskStartParameters, CancellationToken cancellationToken)
		{
			await this._asyncReportDataStoreService.CleanUpAsync();
			return null;
		}
	}
}
