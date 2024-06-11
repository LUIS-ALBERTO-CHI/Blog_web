using FwaEu.Modules.MasterData;
using FwaEu.Modules.ReportsMasterDataByEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportFilterDataProvider
	{
		public Task<IEnumerable<IReportFilterModel>> GetModelsAsync();
	}
}
