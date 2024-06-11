using FwaEu.Fwamework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.WebApi
{
	[Authorize]
	[ApiController]
	[Route("Reports/UserViews")]
	public class ReportsUserViewsController : ControllerBase
	{
		//GET /Reports/UserViews/IsEnabled
		[HttpGet("IsEnabled")]
		[ProducesResponseType(typeof(ReportUserViewIsEnabledModel), StatusCodes.Status200OK)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public ActionResult<ReportUserViewIsEnabledModel> IsEnabled(
			[FromServices] IServiceProvider serviceProvider)
		{
			var viewService = serviceProvider.GetService<IReportUserViewService>();
			return new ReportUserViewIsEnabledModel()
			{
				IsEnabled = viewService != null
			};
		}

		//GET /ReportsUserViews
		[HttpGet("")]
		[ProducesResponseType(typeof(ReportUserViewModel), StatusCodes.Status200OK)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<IEnumerable<ReportUserViewModel>> GetAll(
			[FromServices] IReportUserViewService reportUserViewService)
		{
			return await reportUserViewService.GetAllAsync();
		}

		//DELETE /Reports/UserViews/{id}
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportUserViewModel>> Delete(int id,
			[FromServices] IReportUserViewService reportUserViewService)
		{
			try
			{
				await reportUserViewService.DeleteAsync(id);
			}
			catch (NotFoundException)
			{
				return NotFound(String.Format("View with id #{0} not found.", id));
			}

			return Ok();
		}

		//PUT /Reports/UserViews/{id}
		[HttpPut("{id?}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportUserViewModel>> Save(
			[FromBody] ReportUserViewModel model,
			[FromServices] IReportUserViewService reportUserViewService,
			int? id = null)
		{
			var viewModel = new ReportUserViewModel()
			{
				Id = id,
				Name = model.Name,
				UserId = model.UserId,
				ReportId = model.ReportId,
				Value = model.Value
			};

			try
			{
				await reportUserViewService.SaveAsync(viewModel);
			}
			catch (NotFoundException)
			{
				return NotFound(String.Format("View with id #{0} not found.", id));
			}
			return Ok();
		}
	}
}
