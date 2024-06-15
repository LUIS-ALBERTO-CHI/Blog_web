using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager
{
	public class TownRegionUserPerimeterProvider : EntityUserPerimeterProviderBase
			<
			TownRegionPerimeterEntity, TownRegionPerimeterEntityRepository,
			TownRegionEntity, int, TownRegionEntityRepository
			>
	{
		public TownRegionUserPerimeterProvider(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override Expression<Func<TownRegionPerimeterEntity, int>> SelectReferenceEntityId()
		{
			return entity => entity.Region.Id;
		}

		protected override Expression<Func<TownRegionPerimeterEntity, bool>> WhereContainsReferenceEntities(int[] ids)
		{
			return entity => ids.Contains(entity.Region.Id);
		}

		protected override Expression<Func<TownRegionPerimeterEntity, bool>> WhereFullAccess()
		{
			return entity => entity.Region == null;
		}

		protected override Expression<Func<TownRegionPerimeterEntity, bool>> WhereNotFullAccess()
		{
			return entity => entity.Region != null;
		}

		protected override TownRegionPerimeterEntity CreatePerimeterEntity(TownRegionEntity referenceEntity)
		{
			return new TownRegionPerimeterEntity()
			{
				Region = referenceEntity,
			};
		}
	}
}
