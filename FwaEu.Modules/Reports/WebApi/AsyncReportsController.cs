using FwaEu.Fwamework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.WebApi
{
	[Authorize]
	[ApiController]
	[Route("Reports/Async/")]
	public class AsyncReportsController : ControllerBase
	{
		//POST /Reports/Async/Enqueue/{invariantId}
		[HttpPost("Enqueue/{invariantId}")]
		[ProducesResponseType(typeof(EnqueueResultModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<EnqueueResultModel>> Enqueue(
			[Required] string invariantId,
			[FromBody] ReportGetDataRequestApiModel requestModel,
			[FromServices] IAsyncReportService asyncReportService,
			[FromServices] FiltersParametersProvider filtersParametersProvider)
		{
			ReportsController.LoadFilters(requestModel, filtersParametersProvider);

			try
			{
				var queueGuid = await asyncReportService.EnqueueLoadDataRequestAsync(invariantId);
				return new EnqueueResultModel(queueGuid);
			}
			catch (NotFoundException)
			{
				return NotFound($"Report not found with invariant id: {invariantId}.");
			}
		}

		//GET /Reports/Async/State/{queueGuid}
		[HttpGet("State/{queueGuid}")]
		[ProducesResponseType(typeof(ProgressResultModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProgressErrorModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProgressResultModel>> GetState(Guid queueGuid,
			[FromServices] IAsyncReportResultService asyncReportResultService)
		{
			try
			{
				var result = await asyncReportResultService.GetStateAsync(queueGuid);

				if (result.ErrorMessage != null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError,
						new ProgressErrorModel(result.ErrorMessage));
				}

				return Ok(new ProgressResultModel(result.State,
					result.QueueDate, result.StartDate,
					result.EndDate, result.NumberOfTasksBefore,
					result.ReportCacheStoreKey));
			}
			catch (NotFoundException)
			{
				return NotFound($"Unknown guid '{queueGuid}' in queue.");
			}
		}

		//GET /Reports/Async/Data/{reportCacheStoreKey}
		[HttpGet("Data/{reportCacheStoreKey}")]
		[ProducesResponseType(typeof(ReportDataApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ReportDataApiModel>> GetData(
			[Required] string reportCacheStoreKey,
			[FromServices] IAsyncReportDataStoreService asyncReportDataStoreService)
		{
			var data = await asyncReportDataStoreService.GetAsync(reportCacheStoreKey);

			if (data == null)
			{
				return NotFound($"Cached data does not exists.");
			}

			return Ok(new ReportDataApiModel(data.Rows));
		}
	}
}
