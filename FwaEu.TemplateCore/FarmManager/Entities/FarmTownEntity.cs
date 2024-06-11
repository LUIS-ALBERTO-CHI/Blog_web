using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.SimpleMasterData;
using System;


namespace FwaEu.TemplateCore.FarmManager.Entities
{
	public class FarmTownEntity : SimpleMasterDataEntityBase
	{
		public TownRegionEntity Region { get; set; }

	}

	public class FarmTownEntityClassMap : SimpleMasterDataEntityBaseClassMap<FarmTownEntity>
	{
		public FarmTownEntityClassMap() : base(new SimpleMasterDataClassMapOptions()
		{
			MapOnlyDefaultCultureForName = true,
			NameMappingConfig = new LocalizableStringMappingConfig()
			{
				// NOTE: Don't set the unique key on localized name as multiple cities can have the same name but in a different region.
				UniqueValueByCulture = false,
				OnColumnMapped = new OnColumnMapped((map, culture, isDefaultCulture) =>
				{
					if (!isDefaultCulture)
					{
						//NOTE: Should not be possible because MapOnlyDefaultCultureForName = true
						throw new NotSupportedException();
					}

				})
			},
		})
		{
			References(entity => entity.Region).Not.Nullable();
		}
	}

	public class FarmTownEntityRepository : SimpleMasterDataEntityBaseRepository<FarmTownEntity>
	{
	}
}
