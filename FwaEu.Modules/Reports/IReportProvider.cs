using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportProvider
	{
		Task<ReportLoadResultModel<ServiceReportModel>> FindByInvariantIdAsync(
			string invariantId, CultureInfo userCulture); 

		Task<IEnumerable<ReportLoadResultModel<ReportListItemModel>>> GetAllAsync(CultureInfo userCulture);
	}
}
