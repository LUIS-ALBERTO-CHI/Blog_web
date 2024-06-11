using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Reports.WebApi;
using FwaEu.Modules.ReportsProvidersByEntities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportAdminService
	{
		Task<ReportAdminModel> FindByInvariantIdAsync(string invariantId);
		Task SaveAsync(string invariantId, ReportAdminModel model);
		Task<string> GetJsonByInvariantIdAsync(string invariantId);
		Task DeleteAsync(string invariantId);
		Task<ReportLoadResultModel<ReportAdminListItemModel>[]> GetAllAsync(CultureInfo culture);
	}

}
