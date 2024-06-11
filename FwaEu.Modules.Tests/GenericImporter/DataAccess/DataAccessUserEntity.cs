using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class DataAccessUserEntity : IEntity
	{
		public int Id { get; protected set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class DataAccessUserEntityClassMap : ClassMap<DataAccessUserEntity>
	{
		public DataAccessUserEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.FirstName).Not.Nullable();
			Map(entity => entity.LastName).Not.Nullable();
			Map(entity => entity.Email).Not.Nullable().Unique();
		}
	}

	public class DataAccessUserEntityRepository : DefaultRepository<DataAccessUserEntity, int>
	{
	}
}
