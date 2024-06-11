using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Entities
{
	public class FarmAnimalCountEntity : IUpdateTracked
	{
		public int Id { get; set; }

		public FarmEntity Farm { get; set; }
		public FarmAnimalSpeciesEntity Species { get; set; }
		public int Quantity { get; set; }
		public DateTime UpdatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }

		public bool IsNew() => Id == 0;
	}

	public class FarmAnimalCountEntityClassMap : ClassMap<FarmAnimalCountEntity>
	{
		public FarmAnimalCountEntityClassMap()
		{
			const string SpeciesAndFarmUniqueKey = "SpeciesAndFarmUniqueKey";
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.Farm).Not.Nullable().UniqueKey(SpeciesAndFarmUniqueKey);
			References(entity => entity.Species).Not.Nullable().UniqueKey(SpeciesAndFarmUniqueKey);
			Map(entity => entity.Quantity).Not.Nullable();
			this.AddUpdateTrackedPropertiesIntoMapping();
		}
	}

	public class FarmAnimalCountEntityRepository : DefaultRepository<FarmAnimalCountEntity, int>
	{
	}
}
