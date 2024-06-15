using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using FwaEu.Modules.SimpleMasterData.GenericAdmin;
using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.GenericAdmin
{
	public class FarmTownModel : SimpleMasterDataGenericAdminModel
	{
		[Required]
		[MasterData("TownRegions")]
		public int? RegionId { get; set; }
	}

	public class FarmTownEntityToModelGenericAdminModelConfiguration
		: SimpleMasterDataEntityToModelGenericAdminModelConfiguration<FarmTownEntity, FarmTownModel>
	{
		public FarmTownEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{
		}

		protected override void MapSortModelPropertiesToEntityProperties(
			GenericAdminColumnNameToEntityPropertyMapper<FarmTownEntity, FarmTownModel> mapper,
			CultureInfo userCulture,
			CultureInfo defaultCulture)
		{
			// NOTE: We should use something like: userCulture?.TwoLetterISOLanguageName ?? defaultCulture.TwoLetterISOLanguageName
			// But in the entity, `MapOnlyDefaultCultureForName = true` is set, so we only have the default culture.
			var masterDataCulture = defaultCulture.TwoLetterISOLanguageName;

			mapper
				.MapModelPropertyToEntityProperty(m => m.RegionId, e => e.Region.Name[masterDataCulture] as string)
				.MapModelPropertyToLocalizableEntityProperty(m => m.Name, (e, locale) => e.Name[locale] as string)
				.MapModelPropertyToEntityProperty(m => m.UpdatedById, e => e.UpdatedBy.Identity)
				.MapModelPropertyToEntityProperty(m => m.CreatedById, e => e.CreatedBy.Identity);

			base.MapSortModelPropertiesToEntityProperties(mapper, userCulture, defaultCulture);
		}

		protected override void MapFilterModelPropertiesToEntityProperties(
			GenericAdminColumnNameToEntityPropertyMapper<FarmTownEntity, FarmTownModel> mapper,
			CultureInfo userCulture,
			CultureInfo defaultCulture)
		{
			mapper
				.MapModelPropertyToEntityProperty(m => m.RegionId, e => e.Region.Id)
				.MapModelPropertyToLocalizableEntityProperty(m => m.Name, (e, locale) => e.Name[locale] as string)
				.MapModelPropertyToEntityProperty(m => m.UpdatedById, e => e.UpdatedBy.Id)
				.MapModelPropertyToEntityProperty(m => m.CreatedById, e => e.CreatedBy.Id);

			base.MapFilterModelPropertiesToEntityProperties(mapper, userCulture, defaultCulture);
		}

		protected override Expression<Func<FarmTownEntity, FarmTownModel>> GetSelectExpression()
		{
			return entity => new FarmTownModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				RegionId = entity.Region.Id,
				UpdatedOn = entity.UpdatedOn,
				UpdatedById = entity.UpdatedBy.Id,
				CreatedById = entity.CreatedBy.Id,
				CreatedOn = entity.CreatedOn,
			};
		}

		protected override async Task FillEntityAsync(FarmTownEntity entity, FarmTownModel model)
		{
			await base.FillEntityAsync(entity, model);
			entity.Region = await this.RepositorySession.Session.GetAsync<TownRegionEntity>(model.RegionId);
		}
	}
}
