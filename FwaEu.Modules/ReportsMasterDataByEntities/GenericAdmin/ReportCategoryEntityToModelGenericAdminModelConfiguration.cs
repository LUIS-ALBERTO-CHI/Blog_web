using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsMasterDataByEntities.GenericAdmin
{

	public class ReportCategoryModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		public string InvariantId { get; set; }

		[Required]
		[LocalizableStringCustomType]
		public Dictionary<string, string> Name { get; set; }

		[LocalizableStringCustomType]
		public Dictionary<string, string> Description { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
	}

	public class ReportCategoryEntityToModelGenericAdminModelConfiguration :
		EntityToModelGenericAdminModelConfiguration<ReportCategoryEntity, int, ReportCategoryModel, MainSessionContext>
	{
		public const string Key = nameof(ReportCategoryEntity);

		public ReportCategoryEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{
		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await Task.FromResult(true);
		}

		protected override Task FillEntityAsync(ReportCategoryEntity entity, ReportCategoryModel model)
		{
			entity.InvariantId = model.InvariantId;
			entity.Name = model.Name.ToStringStringDictionary();
			entity.Description = model.Description.ToStringStringDictionary();

			return Task.CompletedTask;
		}

		protected override async Task<ReportCategoryEntity> GetEntityAsync(ReportCategoryModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<ReportCategoryEntity, ReportCategoryModel>> GetSelectExpression()
		{
			return entity => new ReportCategoryModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				Description = entity.Description.ToStringStringDictionary(),
				UpdatedOn = entity.UpdatedOn
			};
		}

		protected override bool IsNew(ReportCategoryModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(ReportCategoryModel model, ReportCategoryEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}
