using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Reports;
using FwaEu.Modules.SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsSearchEngine
{
	public class ReportSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly IReportService _reportService;
		private readonly IUserContextLanguage _userContextLanguage;

		public ReportSearchEngineResultProvider(IReportService reportService, IUserContextLanguage userContextLanguage)
		{
			this._reportService = reportService
				?? throw new ArgumentNullException(nameof(reportService));

			this._userContextLanguage = userContextLanguage
				?? throw new ArgumentNullException(nameof(userContextLanguage));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			var culture = this._userContextLanguage.GetCulture();
			var reports = await this._reportService.GetAllAsync(culture);

			return reports
				.Select(r => r.Report)
				.Where(r => r.InvariantId.Contains(search)
					|| r.Name.Contains(search)
					|| r.Description != null && r.Description.Contains(search))
				.Skip(pagination.Skip)
				.Take(pagination.Take)
				.Select(r => new
				{
					r.InvariantId,
					r.Name,
					r.Description,
					//TODO: Add icon https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/7267
				});
		}
	}
}
