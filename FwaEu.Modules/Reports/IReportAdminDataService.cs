using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public interface IReportAdminDataService 
	{
		DataServiceDataScope LoadDataScope(ReportDataSource dataSource, ParametersModel parameters);
	}

	public class DefaultReportAdminDataService : IReportAdminDataService
	{
		private readonly IDataService dataService;
		public DefaultReportAdminDataService(IDataService dataService)
		{
			this.dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
		}

		public DataServiceDataScope LoadDataScope(ReportDataSource dataSource, ParametersModel parameters)
		{
			return dataService.LoadDataScope(dataSource,parameters);
		}
	}
}
