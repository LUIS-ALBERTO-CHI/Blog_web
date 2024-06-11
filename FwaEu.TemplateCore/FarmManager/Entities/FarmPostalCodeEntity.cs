using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Linq;

namespace FwaEu.TemplateCore.FarmManager.Entities
{
	public class FarmPostalCodeEntity : IEntity, ICreationAndUpdateTracked
	{
		public int Id { get; set; }
		public string PostalCode { get; set; }
		public FarmTownEntity Town { get; set; }
		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return Id == 0;
		}
	}
	public class FarmPostalCodeEntityClassMap : ClassMap<FarmPostalCodeEntity>
	{
		const string TownAndPostalCodeUniqueKey = "UQ_Town_PostalCode";
		public FarmPostalCodeEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.PostalCode).Not.Nullable().UniqueKey(TownAndPostalCodeUniqueKey);
			References(entity => entity.Town).Not.Nullable().UniqueKey(TownAndPostalCodeUniqueKey);
			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}

	public class FarmPostalCodeEntityRepository : DefaultRepository<FarmPostalCodeEntity, int>, IQueryByIds<FarmPostalCodeEntity, int>
	{
		public IQueryable<FarmPostalCodeEntity> QueryByIds(int[] ids)
		{
			return Query().Where(entity => ids.Contains(entity.Id));
		}
	}

}
