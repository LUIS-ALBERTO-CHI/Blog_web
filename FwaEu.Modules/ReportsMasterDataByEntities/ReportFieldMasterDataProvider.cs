using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Reports;
using FwaEu.Modules.Reports.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public class ReportFieldMasterDataProvider : EntityMasterDataProvider<ReportFieldEntity, int,
		ReportFieldMasterDataModel, ReportFieldEntityRepository>
	{
		public ReportFieldMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
		: base(sessionContext, culturesService)
		{

		}

		protected override Expression<Func<ReportFieldEntity, ReportFieldMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new ReportFieldMasterDataModel(entity.InvariantId,
				(string)entity.Name[userCulture.TwoLetterISOLanguageName]
				?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName],
				entity.DataSourceArgument, entity.DataSourceType, entity.UrlFormat, entity.TemplateName, entity.SummaryType,
				entity.DotNetTypeName, (string)(entity.DisplayFormat[userCulture.TwoLetterISOLanguageName]
					?? entity.DisplayFormat[defaultCulture.TwoLetterISOLanguageName]));
		}
	}

	public class ReportFieldMasterDataModel
	{
		public ReportFieldMasterDataModel(string invariantId, string name,
		string dataSourceArgument, string reportDataSourceType, string urlFormat, string templateName,
		ReportSummaryType summaryType, string dotNetTypeName, string displayFormat)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.DataSourceArgument = reportDataSourceType != "Sql" ? dataSourceArgument : null;
			this.DataSourceType = reportDataSourceType;
			this.UrlFormat = urlFormat;
			this.TemplateName = templateName;
			this.SummaryType = summaryType;
			this.DotNetTypeName = dotNetTypeName ?? throw new ArgumentNullException(nameof(dotNetTypeName));
			this.DisplayFormat = displayFormat;
		}

		public string InvariantId { get; }
		public string Name { get; }
		public string DataSourceArgument { get; }
		public string DataSourceType { get; }
		public string UrlFormat { get; }
		public string TemplateName { get; }
		public ReportSummaryType SummaryType { get; }
		public string DotNetTypeName { get; }
		public string DisplayFormat { get; }
	}
}
