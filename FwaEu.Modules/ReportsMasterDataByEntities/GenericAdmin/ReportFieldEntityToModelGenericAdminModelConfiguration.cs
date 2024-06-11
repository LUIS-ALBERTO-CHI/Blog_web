using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsMasterDataByEntities.GenericAdmin
{
	public class ReportFieldModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		public string InvariantId { get; set; }

		[Required]
		[LocalizableStringCustomType]
		public Dictionary<string, string> Name { get; set; }

		[Required]
		public string DotNetTypeName { get; set; }

		public string DataSourceType { get; set; }

		public string DataSourceArgument { get; set; }

		[LocalizableStringCustomType]
		public Dictionary<string, string> DisplayFormat { get; set; }

		public string UrlFormat { get; set; }

		public string TemplateName { get; set; }

		[Required]
		[EnumMasterData(typeof(ReportSummaryType))]
		public ReportSummaryType SummaryType { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }
	}

	public class ReportFieldEntityToModelGenericAdminModelConfiguration
		: EntityToModelGenericAdminModelConfiguration<ReportFieldEntity, int, ReportFieldModel, MainSessionContext>
	{
		public const string Key = nameof(ReportFieldEntity);

		public ReportFieldEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{

		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await Task.FromResult(true);
		}

		protected override Task FillEntityAsync(ReportFieldEntity entity, ReportFieldModel model)
		{
			entity.InvariantId = model.InvariantId;
			entity.Name = model.Name.ToStringStringDictionary();
			entity.DotNetTypeName = model.DotNetTypeName;
			entity.DataSourceType = model.DataSourceType;
			entity.DataSourceArgument = model.DataSourceArgument;
			entity.DisplayFormat = model.DisplayFormat.ToStringStringDictionary();
			entity.UrlFormat = model.UrlFormat;
			entity.TemplateName = model.TemplateName;
			entity.SummaryType = model.SummaryType;

			return Task.CompletedTask;
		}

		protected override async Task<ReportFieldEntity> GetEntityAsync(ReportFieldModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<ReportFieldEntity, ReportFieldModel>> GetSelectExpression()
		{
			return entity => new ReportFieldModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				DotNetTypeName = entity.DotNetTypeName,
				DataSourceType = entity.DataSourceType,
				DataSourceArgument = entity.DataSourceArgument,
				DisplayFormat = entity.DisplayFormat.ToStringStringDictionary(),
				UrlFormat = entity.UrlFormat,
				TemplateName = entity.TemplateName,
				SummaryType = entity.SummaryType,
				UpdatedById = entity.UpdatedBy.Id,
				UpdatedOn = entity.UpdatedOn
			};
		}

		protected override bool IsNew(ReportFieldModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(ReportFieldModel model, ReportFieldEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}
