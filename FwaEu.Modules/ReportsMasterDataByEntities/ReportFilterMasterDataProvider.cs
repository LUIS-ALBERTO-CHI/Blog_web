using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Reports;
using FwaEu.Modules.Reports.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public class ReportFilterMasterDataProvider : EntityMasterDataProvider<ReportFilterEntity, int,
		ReportFilterMasterDataModel, ReportFilterEntityRepository>
	{

		public ReportFilterMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
				: base(sessionContext, culturesService)
		{

		}
		protected override Expression<Func<ReportFilterEntity, ReportFilterMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new ReportFilterMasterDataModel(entity.InvariantId,
				(string)entity.Name[userCulture.TwoLetterISOLanguageName]
				?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName],
				entity.DataSourceArgument, entity.DataSourceType, entity.DotNetTypeName);
		}
	}

	public class ReportFilterMasterDataModel : IReportFilterModel
	{
		public ReportFilterMasterDataModel(string invariantId, string name, string dataSourceArgument, string reportDataSourceType, string dotNetTypeName)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.DataSourceArgument = reportDataSourceType != "Sql" ? dataSourceArgument : null;
			this.DataSourceType = reportDataSourceType;
			this.DotNetTypeName = dotNetTypeName ?? throw new ArgumentNullException(nameof(dotNetTypeName));
		}

		public string InvariantId { get; }
		public string Name { get; }
		public string DataSourceArgument { get; }
		public string DataSourceType { get; }
		public string DotNetTypeName { get; }
	}

	public interface IReportFilterModel
	{
		string InvariantId { get; }
		string Name { get; }
		string DotNetTypeName { get; }
	}
}
