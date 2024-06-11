using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
	[Route("[controller]")]
	[RequirePermissions(nameof(ReportPermissionProvider.CanViewReports))]
	public class ReportsController : ControllerBase
	{
		private static ReportNavigationItemApiModel CreateReportNavigationItemApiModel(ReportNavigationItemModel model)
		{
			return new ReportNavigationItemApiModel(model.Visible, model.Index);
		}

		private static ReportListItemApiModel CreateReportListItemApiModel(ReportLoadResultModel<ReportListItemModel> model)
		{
			var report = model.Report;

			return new ReportListItemApiModel(report.InvariantId, report.HasFilters, report.Name,
				report.Description, report.CategoryInvariantId,
				new ReportNavigationApiModel(
					CreateReportNavigationItemApiModel(report.Navigation.Menu),
					CreateReportNavigationItemApiModel(report.Navigation.Summary)
					),
				report.IsAsync, report.Icon);
		}

		private static ReportApiModel CreateReportApiModel(ReportLoadResultModel<ServiceReportModel> model)
		{
			return new ReportApiModel(model.Report.Name, model.Report.Description, model.Report.CategoryInvariantId,
				new ReportNavigationApiModel(CreateReportNavigationItemApiModel(model.Report.Navigation.Menu),
					CreateReportNavigationItemApiModel(model.Report.Navigation.Summary)), model.Report.IsAsync, model.Report.Icon,
				new ReportDataSourceApiModel(model.Report.DataSource.Type, model.Report.DataSource.Argument), 
				model.Report.Filters.Select(x => new ReportFilterApiModel(x.InvariantId, x.IsRequired)).ToArray(),
				model.Report.Properties.Select(x => new ReportPropertyApiModel(x.Name, x.FieldInvariantId)).ToArray(),
				model.Report.DefaultViews.ToDictionary(x => x.Key,
					x => x.Value.Select(v => new ReportViewApiModel(v.IsDefault, v.Name, v.Value)).ToArray()));
		}

		public static void LoadFilters(ReportGetDataRequestApiModel requestModel,
			FiltersParametersProvider filtersParametersProvider)
		{
			if (requestModel.Filters != null)
			{
				var filters = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(
					requestModel.Filters.ToString());

				filtersParametersProvider.SetFilters(filters);
			}
		}

		//POST /Reports/
		[HttpPost("")]
		[ProducesResponseType(typeof(ReportListItemApiModel[]), StatusCodes.Status200OK)]
		public async Task<ActionResult<ReportListItemApiModel[]>> GetAll(
			[FromBody] ReportRequestApiModel requestModel,
			[FromServices] IReportService reportService,
			[FromServices] IUserContextLanguage userContextLanguage)
		{
			var culture = userContextLanguage.GetCulture();
			var reports = await reportService.GetAllAsync(culture);

			return Ok(reports.Select(CreateReportListItemApiModel).ToArray());
		}

		//POST /Reports/{invariantId}
		[HttpPost("{invariantId}")]
		[ProducesResponseType(typeof(ReportApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ReportApiModel>> FindByInvariantIdAsync(string invariantId,
			[FromBody]ReportRequestApiModel requestModel,
			[FromServices]IReportService reportService,
			[FromServices]IUserContextLanguage userContextLanguage)
		{
			var culture = userContextLanguage.GetCulture();
			var report = await reportService.FindByInvariantIdAsync(invariantId, culture);

			if (report != null)
			{
				var reportApiModel = CreateReportApiModel(report);

				return Ok(reportApiModel);
			}
			return NotFound($"Report not found with invariant id: {invariantId}.");
		}

		//POST /Reports/{invariantId}/Data
		[HttpPost("{invariantId}/Data")]
		[ProducesResponseType(typeof(ReportDataApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ReportDataApiModel>> GetData(string invariantId,
			[FromBody] ReportGetDataRequestApiModel requestModel,
			[FromServices] IReportDataService reportDataService,
			[FromServices] FiltersParametersProvider filtersParametersProvider)
		{
			LoadFilters(requestModel, filtersParametersProvider);

			try
			{
				var dataResult = await reportDataService.LoadDataAsync(invariantId, CancellationToken.None);
				return new ReportDataApiModel(dataResult.Rows);
			}
			catch (NotFoundException)
			{
				return NotFound($"Report not found with invariant id: {invariantId}.");
			}
		}
	}
}
