using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Globalization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportDataService
	{
		Task<ReportDataModel> LoadDataAsync(string reportInvariantId, CancellationToken cancellationToken);
	}

	public class DefaultReportDataService : IReportDataService
	{
		private readonly IReportService _reportService;
		private readonly ICulturesService _culturesService;
		private readonly IParametersService _parametersService;
		private readonly IReportParametersValidationService _reportParametersValidationService;
		private readonly IReportDataLoadingService _reportDataLoadingService;

		public DefaultReportDataService(
			IReportService reportService,
			ICulturesService culturesService,
			IParametersService parametersService,
			IReportParametersValidationService reportParametersValidationService,
			IReportDataLoadingService reportDataLoadingService)
		{
			this._reportService = reportService
				?? throw new ArgumentNullException(nameof(reportService));

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));

			this._parametersService = parametersService
				?? throw new ArgumentNullException(nameof(parametersService));

			this._reportParametersValidationService = reportParametersValidationService
				?? throw new ArgumentNullException(nameof(reportParametersValidationService));

			this._reportDataLoadingService = reportDataLoadingService
				?? throw new ArgumentNullException(nameof(reportDataLoadingService));
		}

		public async Task<ReportDataModel> LoadDataAsync(string reportInvariantId, CancellationToken cancellationToken)
		{
			var loadReportResult = await this._reportService.FindByInvariantIdAsync(
				reportInvariantId, this._culturesService.DefaultCulture);

			if (loadReportResult == null)
			{
				throw new NotFoundException($"Report not found with invariant id: {reportInvariantId}.");
			}

			var parameters = await _parametersService.LoadParametersAsync();
			await this._reportParametersValidationService.ValidateAsync(parameters, loadReportResult);

			return await this._reportDataLoadingService.LoadDataAsync(
				loadReportResult.Report, parameters, cancellationToken);
		}
	}
}
