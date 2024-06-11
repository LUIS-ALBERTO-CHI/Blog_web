using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Entities
{
	public class TownRegionPerimeterEntity : IPerimeterEntity<int>
	{
		public const string ProviderKey = "RegionPerimeters";

		public int Id { get; protected set; }

		public TownRegionEntity Region { get; set; }
		public UserEntity User { get; set; }

		public int GetReferenceEntityId()
		{
			return Region.Id;
		}

		public bool IsNew()
		{
			return Id == 0;
		}
	}

	public class RegionPerimeterEntityClassMap : ClassMap<TownRegionPerimeterEntity>
	{
		public RegionPerimeterEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.Region).Not.Nullable().UniqueKey("UQ_Town_Region");
			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_Town_Region");

		}
	}

	public class TownRegionPerimeterEntityRepository : DefaultRepository<TownRegionPerimeterEntity, int>
	{
	}
}
