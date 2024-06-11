using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.BackgroundTasks;
using FwaEu.Modules.Reports.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IAsyncReportService
	{
		Task<Guid> EnqueueLoadDataRequestAsync(string reportInvariantId);
	}

	public class DefaultAsyncReportService : IAsyncReportService
	{
		private readonly IBackgroundTasksService _backgroundTasksService;
		private readonly IReportService _reportService;
		private readonly ICulturesService _culturesService;
		private readonly IParametersService _parametersService;
		private readonly IReportParametersValidationService _reportParametersValidationService;

		public DefaultAsyncReportService(
			IBackgroundTasksService backgroundTasksService,
			IReportService reportService,
			ICulturesService culturesService,
			IParametersService parametersService,
			IReportParametersValidationService reportParametersValidationService)
		{
			this._backgroundTasksService = backgroundTasksService
				?? throw new ArgumentNullException(nameof(backgroundTasksService));

			this._reportService = reportService
				?? throw new ArgumentNullException(nameof(reportService));

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));

			this._parametersService = parametersService
				?? throw new ArgumentNullException(nameof(parametersService));

			this._reportParametersValidationService = reportParametersValidationService
				?? throw new ArgumentNullException(nameof(reportParametersValidationService));
		}

		public async Task<Guid> EnqueueLoadDataRequestAsync(string reportInvariantId)
		{
			var loadReportResult = await this._reportService.FindByInvariantIdAsync(
				reportInvariantId, this._culturesService.DefaultCulture);

			if (loadReportResult == null)
			{
				throw new NotFoundException($"Report not found with invariant id: {reportInvariantId}.");
			}

			var parameters = await _parametersService.LoadParametersAsync();
			await this._reportParametersValidationService.ValidateAsync(parameters, loadReportResult);

			var taskParameters = new ReportLoadDataParametersModel(reportInvariantId, parameters.Parameters);

			var taskStartParameters = new TaskStartParameters(
				ReportLoadDataBackgroundTask.TaskName,
				taskParameters.Serialize());

			return await this._backgroundTasksService.EnqueueAsync(taskStartParameters);
		}
	}
}