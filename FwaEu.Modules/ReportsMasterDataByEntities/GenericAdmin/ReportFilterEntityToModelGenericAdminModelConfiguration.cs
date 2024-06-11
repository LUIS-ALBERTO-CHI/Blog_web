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
	public class ReportFilterModel
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

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }
	}
	public class ReportFilterEntityToModelGenericAdminModelConfiguration
		: EntityToModelGenericAdminModelConfiguration<ReportFilterEntity, int, ReportFilterModel, MainSessionContext>
	{
		public const string Key = nameof(ReportFilterEntity);

		public ReportFilterEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{

		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await Task.FromResult(true);
		}

		protected override Task FillEntityAsync(ReportFilterEntity entity, ReportFilterModel model)
		{
			entity.InvariantId = model.InvariantId;
			entity.Name = model.Name.ToStringStringDictionary();
			entity.DotNetTypeName = model.DotNetTypeName;
			entity.DataSourceArgument = model.DataSourceArgument;
			entity.DataSourceType = model.DataSourceType;

			return Task.CompletedTask;
		}

		protected override async Task<ReportFilterEntity> GetEntityAsync(ReportFilterModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<ReportFilterEntity, ReportFilterModel>> GetSelectExpression()
		{
			return entity => new ReportFilterModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				DotNetTypeName = entity.DotNetTypeName,
				DataSourceType = entity.DataSourceType,
				DataSourceArgument = entity.DataSourceArgument,
				UpdatedOn = entity.UpdatedOn,
				UpdatedById = entity.UpdatedBy.Id
			};
		}

		protected override bool IsNew(ReportFilterModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(ReportFilterModel model, ReportFilterEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}
