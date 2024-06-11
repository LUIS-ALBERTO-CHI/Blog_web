using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportDataLoadingService
	{
		Task<ReportDataModel> LoadDataAsync(
			ServiceReportModel report,
			ParametersModel parameters,
			CancellationToken cancellationToken);

		Task<ReportDataModel> LoadDataForPropertiesAsync(
			ReportDataSource reportDataSource,
			ParametersModel parameters,
			CancellationToken cancellationToken);
	}

	public class DefaultReportDataLoadingService : IReportDataLoadingService
	{
		private readonly IDataService _dataService;
		private readonly ILogger<DefaultReportDataService> _logger;

		public DefaultReportDataLoadingService(IDataService dataService, ILogger<DefaultReportDataService> logger)
		{
			this._dataService = dataService
				?? throw new ArgumentNullException(nameof(dataService));

			this._logger = logger
				?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ReportDataModel> LoadDataForPropertiesAsync(
			ReportDataSource reportDataSource,
			ParametersModel parameters,
			CancellationToken cancellationToken)
		{
			var dataScope = this._dataService.LoadDataScope(reportDataSource, parameters);
			try
			{
				var data = await dataScope.LoadDataTask(cancellationToken);

				return data;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, string.Format("Load data for report admin '{0}' failed after {1:ss\\.ffff} s.", "robert"));

				throw;
			}
		}

		public async Task<ReportDataModel> LoadDataAsync(
		ServiceReportModel report,
		ParametersModel parameters,
		CancellationToken cancellationToken)
		{
			var dataScope = this._dataService.LoadDataScope(report.DataSource, parameters);

			var logScope = dataScope.LogScope.Concat(parameters.Parameters)
				.ToDictionary(x => x.Key, x => x.Value);

			using (_logger.BeginScope(logScope))
			{
				var timer = new Stopwatch();
				timer.Start();

				TimeSpan StopTimerAndAddDurationToLogScopeThenReturnTimeSpan()
				{
					timer.Stop();
					var timeStamp = timer.Elapsed;
					logScope.Add("Duration", timeStamp);
					return timeStamp;
				}

				try
				{
					var data = await dataScope.LoadDataTask(cancellationToken);
					var timeSpan = StopTimerAndAddDurationToLogScopeThenReturnTimeSpan();
					_logger.LogDebug(string.Format("Load data for report '{0}' successfuly executed in {1:ss\\.ffff} s.",
						report.InvariantId, timeSpan));

					return data;
				}
				catch (Exception ex)
				{
					var timeSpan = StopTimerAndAddDurationToLogScopeThenReturnTimeSpan();
					_logger.LogError(ex, string.Format("Load data for report '{0}' failed after {1:ss\\.ffff} s.",
						report.InvariantId, timeSpan));

					throw;
				}
			}

		}
	}
}
