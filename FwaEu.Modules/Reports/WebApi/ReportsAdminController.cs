using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports.WebApi
{
	[Authorize]
	[ApiController]
	[Route("/Reports/Admin")]
	[RequirePermissions(nameof(ReportAdminPermissionProvider.CanAdministrateReports))]
	public class ReportsAdminController : ControllerBase
	{
		//GET /Reports/
		[HttpGet("")]
		[ProducesResponseType(typeof(ReportAdminListItemApiModel[]), StatusCodes.Status200OK)]
		public async Task<ActionResult<ReportAdminListItemApiModel[]>> GetAll(
			[FromServices] IReportAdminService reportService,
			[FromServices] IUserContextLanguage userContextLanguage)
		{
			var culture = userContextLanguage.GetCulture();
			var reports = await reportService.GetAllAsync(culture);

			return Ok(reports.Select(CreateReportAdminListItemApiModel).ToArray());
		}

		private static ReportAdminListItemApiModel CreateReportAdminListItemApiModel(ReportLoadResultModel<ReportAdminListItemModel> model)
		{
			var report = model.Report;

			return new ReportAdminListItemApiModel(report.Name,
				report.CategoryInvariantId,
				report.InvariantId,
				report.Icon,
				report.SupportSave);
		}

		//GET /Reports/Admin/invariantId 
		[HttpGet("{invariantId}")]
		[ProducesResponseType(typeof(ReportAdminApiModel), StatusCodes.Status200OK)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportAdminApiModel>> FindByInvariantIdAsync(string invariantId,
			[FromServices] IReportAdminService reportAdminService)
		{
			var report = await reportAdminService.FindByInvariantIdAsync(invariantId);

			if (report == null)
			{
				return NotFound($"Report not found with invariant id: {invariantId}.");
			}

			return Ok(CreateReportAdminApiModel(report));
		}

		//GET /Reports/Admin/{invariantId}/RawJson
		[HttpGet("{invariantId}/RawJson")]
		[ProducesResponseType(typeof(ReportAdminApiModel), StatusCodes.Status200OK)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportAdminApiModel>> RawJson(string invariantId,
			[FromServices] IReportAdminService reportAdminService)
		{
			var reportJson = await reportAdminService.GetJsonByInvariantIdAsync(invariantId);
			if (reportJson == null)
				return NotFound($"Report not found with invariant id: {invariantId}.");
			return Ok(reportJson);
		}

		//POST /Reports/Admin/{invariantId}
		[HttpPost("{invariantId}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status501NotImplemented)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportAdminApiModel>> SaveAsync(string invariantId,
			[FromServices] IReportAdminService reportAdminService,
			[FromBody] ReportAdminApiModel model)
		{
			try
			{
				await reportAdminService.SaveAsync(invariantId, CreateReportAdminModel(model));
			}
			catch (NotSupportedException)
			{
				return StatusCode(StatusCodes.Status501NotImplemented, "Save is not supported for this report.");
			}

			return Ok();
		}

		//DELETE /Reports/Admin/{invariantId}
		[HttpDelete("{invariantId}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status501NotImplemented)]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportAdminApiModel>> DeleteAsync(string invariantId,
			[FromServices] IReportAdminService reportAdminService)
		{
			try
			{
				await reportAdminService.DeleteAsync(invariantId);
			}
			catch (NotSupportedException)
			{
				return StatusCode(StatusCodes.Status501NotImplemented, "Save is not supported for this report.");
			}

			return Ok();
		}

		//POST /Reports/Admin/Data 
		[HttpPost("Data")]
		[SwaggerOperation(Tags = new[] { "Reports" })]
		public async Task<ActionResult<ReportDataApiModel>> GetData(
			[FromBody] ReportAdminLoadDataSource loadDataSource,
			[FromServices] IReportAdminDataService reportAdminDataService,
			[FromServices] IParametersService parametersService)
		{
			var parameters = new ParametersModel(new ReadOnlyDictionary<string, object>(
				loadDataSource.Parameters
					.ToDictionary(p => p.InvariantId, p => p.Value)
				));

			var report = await reportAdminDataService
				.LoadDataScope(new ReportDataSource(loadDataSource.Type, loadDataSource.Argument), parameters)
				.LoadDataTask(CancellationToken.None);

			return Ok(new ReportDataApiModel(report.Rows));
		}

		private ReportAdminModel CreateReportAdminModel(ReportAdminApiModel report)
		{
			return new ReportAdminModel(report.Name, report.Description, report.CategoryInvariantId, report.IsAsync, report.Icon,
			new ReportAdminNavigationModel(CreateReportNavigationItemModel(report.Navigation.Menu),
			CreateReportNavigationItemModel(report.Navigation.Summary)),
			new ReportAdminDataSourceModel(report.DataSource.Type, report.DataSource.Argument),
			report.Filters.Select(x => new ReportAdminFilterModel(x.InvariantId, x.IsRequired)).ToArray(),
			report.Properties.Select(x => new ReportAdminPropertyModel(x.Name, x.FieldInvariantId)).ToArray(),
			report.DefaultViews.ToDictionary(x => x.Key, x => x.Value.Select(v => new ReportAdminViewModel(v.IsDefault, v.Name, v.Value)).ToArray())) ;
		}

		private ReportAdminApiModel CreateReportAdminApiModel(ReportAdminModel report)
		{
			return new ReportAdminApiModel(report.Name, report.Description, report.CategoryInvariantId, report.IsAsync, report.Icon,
				new ReportAdminNavigationApiModel(CreateReportNavigationItemApiModel(report.Navigation.Menu),
				CreateReportNavigationItemApiModel(report.Navigation.Summary)),
				new ReportAdminDataSourceApiModel(report.DataSource.Type, report.DataSource.Argument),
				report.Filters.Select(x => new ReportAdminFilterApiModel(x.InvariantId, x.IsRequired)).ToArray(),
				report.Properties.Select(x => new ReportAdminPropertyApiModel(x.Name, x.FieldInvariantId)).ToArray(),
				report.DefaultViews.ToDictionary(x => x.Key, x => x.Value.Select(v => new ReportAdminViewApiModel(v.IsDefault, v.Name, v.Value)).ToArray()));
		}

		private ReportAdminNavigationItemApiModel CreateReportNavigationItemApiModel(ReportAdminNavigationItemModel model)
		{
			return new ReportAdminNavigationItemApiModel(model.Visible, model.Index);
		}

		private ReportAdminNavigationItemModel CreateReportNavigationItemModel(ReportAdminNavigationItemApiModel model)
		{
			return new ReportAdminNavigationItemModel(model.Visible, model.Index);

		}
	}
}
