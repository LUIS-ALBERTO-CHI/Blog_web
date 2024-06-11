using FwaEu.Fwamework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.WebApi
{
	[Authorize]
	[ApiController]
	[Route("Reports/Filters")]
	public class ReportsFiltersController : ControllerBase
	{
		//POST /Reports/Filters/Data/{FilterId}/Data
		[HttpPost("Data/{filterId}/Data")]
		[ProducesResponseType(typeof(ReportDataApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportDataApiModel>> GetData(string filterId,
			[FromBody] ReportGetDataRequestApiModel requestModel,
			[FromServices] IReportFiltersDataService filtersDataService,
			[FromServices] FiltersParametersProvider filtersParametersProvider)
		{
			ReportsController.LoadFilters(requestModel, filtersParametersProvider);

			try
			{
				var dataResult = await filtersDataService.LoadDataAsync(filterId, CancellationToken.None);
				return new ReportDataApiModel(dataResult.Rows);
			}
			catch (NotFoundException)
			{
				return NotFound($"Filter not found with invariant id: {filterId}.");
			}
		}
	}
}
