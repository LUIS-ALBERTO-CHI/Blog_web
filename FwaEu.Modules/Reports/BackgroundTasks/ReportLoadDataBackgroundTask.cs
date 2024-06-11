using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.BackgroundTasks;
using FwaEu.Modules.UserNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.BackgroundTasks
{
	public class ReportLoadDataBackgroundTask : IBackgroundTask
	{
		public const string TaskName = "ReportLoadData";

		private readonly IReportService _reportService;
		private readonly ICultureResolver _cultureResolver;
		private readonly IReportDataLoadingService _reportDataLoadingService;
		private readonly IAsyncReportDataStoreService _asyncReportDataStoreService;
		private readonly IUserNotificationService _userNotificationService;

		public ReportLoadDataBackgroundTask(
			IReportService reportService,
			ICultureResolver cultureResolver,
			IReportDataLoadingService reportDataLoadingService,
			IAsyncReportDataStoreService asyncReportDataStoreService,
			IUserNotificationService userNotificationService)
		{
			this._reportService = reportService
				?? throw new ArgumentNullException(nameof(reportService));

			this._cultureResolver = cultureResolver
				?? throw new ArgumentNullException(nameof(cultureResolver));

			this._reportDataLoadingService = reportDataLoadingService
				?? throw new ArgumentNullException(nameof(reportDataLoadingService));

			this._asyncReportDataStoreService = asyncReportDataStoreService
				?? throw new ArgumentNullException(nameof(asyncReportDataStoreService));

			this._userNotificationService = userNotificationService
				?? throw new ArgumentNullException(nameof(userNotificationService));
		}

		public async Task<string> ExecuteAsync(ITaskStartParameters taskStartParameters, CancellationToken cancellationToken)
		{
			var model = ReportLoadDataParametersModel.Deserialize(taskStartParameters.Argument);

			var userCulture = this._cultureResolver.ResolveBestCulture(
				new[] { (string)model.Parameters[CultureParametersProvider.UserCultureTwoLetterISOLanguageNameKey] });

			var loadReportResult = await this._reportService.FindByInvariantIdAsync(
				model.ReportInvariantId, userCulture, checkPerimeter: false);

			if (loadReportResult == null)
			{
				throw new NotFoundException($"Report not found with invariant id: {model.ReportInvariantId}.");
			}

			var parameters = new ParametersModel(model.Parameters);

			var data = await this._reportDataLoadingService.LoadDataAsync(
				loadReportResult.Report, parameters, cancellationToken);

			var reportCacheStoreKey = await this._asyncReportDataStoreService.SetAsync(data);

			var filterKeys = loadReportResult.Report.Filters.Select(f => f.InvariantId).ToArray();

			var filterParameters = model.Parameters
				.Where(kv => filterKeys.Contains(kv.Key))
				.ToDictionary(kv => kv.Key, kv => kv.Value);

			await this._userNotificationService.SendNotificationAsync("ReportLoaded", new
			{
				ReportInvariantId = loadReportResult.Report.InvariantId,
				ReportName = loadReportResult.Report.Name,
				ReportCacheStoreKey = reportCacheStoreKey,
				FilterParameters = filterParameters,
			},
			(string)model.Parameters[CurrentUserParametersProvider.CurrentUserIdentityKey]);

			return reportCacheStoreKey;
		}
	}
}