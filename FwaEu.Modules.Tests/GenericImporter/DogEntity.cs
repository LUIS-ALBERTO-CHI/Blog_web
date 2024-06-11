using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class DogEntity : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Birthdate { get; set; }
		public decimal? Price { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class DogEntityClassMap : ClassMap<DogEntity>
	{
		public DogEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.Name).Not.Nullable().Unique();
			Map(entity => entity.Birthdate).Not.Nullable();
			Map(entity => entity.Price).Nullable();
		}
	}

	public class DogEntityRepository : DefaultRepository<DogEntity, int>
	{
	}
}
