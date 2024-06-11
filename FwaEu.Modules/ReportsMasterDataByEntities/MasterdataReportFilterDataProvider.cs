using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Reports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public class MasterDataReportFilterDataProvider : IReportFilterDataProvider
	{
		private readonly ReportFilterMasterDataProvider _reportFilterMasterDataProvider;
		private readonly IUserContextLanguage _userContextLanguage;

		public MasterDataReportFilterDataProvider(ReportFilterMasterDataProvider reportFilterMasterDataProvider,
			IUserContextLanguage userContextLanguage)
		{
			this._reportFilterMasterDataProvider = reportFilterMasterDataProvider ??
					throw new ArgumentNullException(nameof(reportFilterMasterDataProvider));
			this._userContextLanguage = userContextLanguage ?? throw new ArgumentNullException(nameof(userContextLanguage));
		}

		public async Task<IEnumerable<IReportFilterModel>> GetModelsAsync()
		{
			var models = await this._reportFilterMasterDataProvider
				.GetModelsAsync(new MasterDataProviderGetModelsParameters(
					null, null, null, _userContextLanguage.GetCulture()));

			return models;
		}
	}
}
