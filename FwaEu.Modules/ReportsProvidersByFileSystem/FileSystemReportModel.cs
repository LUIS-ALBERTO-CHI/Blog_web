using FwaEu.Modules.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsProvidersByFileSystem
{
	public class FileSystemReportModel : ReportModel<ReportLocalizableString>
	{
		public FileSystemReportModel(string invariantId, ReportLocalizableString name, ReportLocalizableString description,
			string categoryInvariantId, ReportNavigationModel navigation, bool isAsync, string icon,
			ReportDataSource dataSource, ReportFilterModel[] filters, ReportPropertyModel[] properties,
			Dictionary<string, ReportViewModel<ReportLocalizableString>[]> defaultViews)
			: base(name, description,
				categoryInvariantId, isAsync, icon, navigation, 
				dataSource, filters, properties,
				defaultViews)
		{
		}
	}
}
