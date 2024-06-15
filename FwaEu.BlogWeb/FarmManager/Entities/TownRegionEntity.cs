using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.SimpleMasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Entities
{
	public class TownRegionEntity : SimpleMasterDataEntityBase
	{

	}
	public class TownRegionEntityClassMap : SimpleMasterDataEntityBaseClassMap<TownRegionEntity>
	{
		public TownRegionEntityClassMap() : base(new SimpleMasterDataClassMapOptions()
		{
			MapOnlyDefaultCultureForName = true,
			NameMappingConfig = new LocalizableStringMappingConfig()
			{
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
		{ }
	}
	public class TownRegionEntityRepository : SimpleMasterDataEntityBaseRepository<TownRegionEntity>
	{
		protected override IEnumerable<IRepositoryDataFilter<TownRegionEntity, int>> CreateDataFilters(
			RepositoryDataFilterContext<TownRegionEntity, int> context)
		{
			yield return new TownRegionEntityDataFilter();
		}
	}
}
