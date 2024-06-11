using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Data.Database.Sessions
{
	public class CatEntity : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Birthdate { get; set; }
		public string Color { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class CatEntityClassmap : ClassMap<CatEntity>
	{
		public CatEntityClassmap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.Name).Not.Nullable().Unique();
			Map(entity => entity.Birthdate).Not.Nullable();
			Map(entity => entity.Color).Not.Nullable();
		}
	}

	public class CatEntityRepository : DefaultRepository<CatEntity, int>
	{
	}
}
