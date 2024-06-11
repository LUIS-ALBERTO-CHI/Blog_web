using FwaEu.Fwamework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.WebApi
{
	[Authorize]
	[ApiController]
	[Route("Reports/Fields")]
	public class ReportsFieldsController : ControllerBase
	{
		//POST /Reports/Fields/Data/{FieldId}/Data
		[HttpPost("Data/{fieldId}/Data")]
		[ProducesResponseType(typeof(ReportDataApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportDataApiModel>> GetData(string fieldId,
			[FromBody] ReportGetDataRequestApiModel requestModel,
			[FromServices] IReportFieldsDataService reportFieldsDataService,
			[FromServices] FiltersParametersProvider filtersParametersProvider)
		{
			ReportsController.LoadFilters(requestModel, filtersParametersProvider);

			try
			{
				var dataResult = await reportFieldsDataService.LoadDataAsync(fieldId, CancellationToken.None);
				return new ReportDataApiModel(dataResult.Rows);
			}
			catch (NotFoundException)
			{
				return NotFound($"Field not found with invariant id: {fieldId}.");
			}
		}
	}
}
