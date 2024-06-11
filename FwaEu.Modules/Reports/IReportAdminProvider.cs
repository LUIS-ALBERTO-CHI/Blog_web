using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportAdminProvider
	{
		Task<ReportLoadResultModel<ReportAdminModel>> GetByInvariantIdAsync(string invariantId);
		bool SupportsSave { get; }
		Task<bool> ReportExistsAsync(string invariantId);
		Task SaveAsync(string invariantId, ReportAdminModel report);
		Task<string> GetJsonByInvariantIdAsync(string invariantId);
		Task DeleteAsync(string invariantId);
		Task<ReportLoadResultModel<ReportAdminListItemModel>[]> GetAllAsync(CultureInfo userCulture);
	}
}
