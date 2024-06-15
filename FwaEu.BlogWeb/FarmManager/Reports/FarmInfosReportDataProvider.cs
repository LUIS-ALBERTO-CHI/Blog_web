using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using FwaEu.BlogWeb.FarmManager.Services;

namespace FwaEu.BlogWeb.FarmManager.Reports
{
	public class FarmInfosReportDataProvider : IReportDataProvider
	{
		private IFarmService _farmService;
		public FarmInfosReportDataProvider(IFarmService farmService)
		{
			this._farmService = farmService;
		}

		public IReadOnlyDictionary<string, object> GetLogScope(string dataSourceArgument)
		{
			return new Dictionary<string, object>();
		}

		public async Task<ReportDataModel> LoadDataAsync(
			string dataSourceArgument,
			ParametersModel parameters,
			CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var rowsList = new List<Dictionary<string, object>>();

			var farms = await this._farmService.GetAllAsync(false);
			foreach (var farm in farms.Where(f => f.MainActivityId == Convert.ToInt32(dataSourceArgument)))
			{
				var row = new Dictionary<string, object>
				{
					["Id"] = farm.Id,
					["AnimalCount"] = farm.AnimalCount,
					["CategorySize"] = farm.CategorySize,
					["ClosingDate"] = farm.ClosingDate,
					["OpeningDate"] = farm.OpeningDate,
					["Name"] = farm.Name,
					["RecruitEmployees"] = farm.RecruitEmployees,
					["SellingPriceInEurosWithoutTaxes"] = farm.SellingPriceInEurosWithoutTaxes,
				};
				rowsList.Add(row);
			}
			return new ReportDataModel(rowsList.ToArray());
		}
	}

	public class FarmInfosReportDataProviderFactory : IReportDataProviderFactory
	{
		public IReportDataProvider Create(string dataSourceType, IServiceProvider serviceProvider)
		{
			if (dataSourceType == "FarmInfos")
			{
				return serviceProvider.GetService<FarmInfosReportDataProvider>();
			}
			return null;
		}
	}
}
