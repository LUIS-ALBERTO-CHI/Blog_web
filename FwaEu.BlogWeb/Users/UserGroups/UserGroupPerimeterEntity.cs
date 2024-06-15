using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.UserGroups
{
	public class UserGroupPerimeterEntity : IPerimeterEntity<int>
	{
		public const string ProviderKey = "UserGroups";

		public int Id { get; protected set; }

		public UserEntity User { get; set; }
		public UserGroupEntity UserGroup { get; set; }

		public int GetReferenceEntityId()
		{
			return this.UserGroup.Id;
		}

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserGroupPerimeterEntityClassMap : ClassMap<UserGroupPerimeterEntity>
	{
		public UserGroupPerimeterEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_User_Group");
			References(entity => entity.UserGroup).UniqueKey("UQ_User_Group"); // NOTE: NULL means full access to user groups
		}
	}

	public class UserGroupPerimeterEntityRepository : DefaultRepository<UserGroupPerimeterEntity, int>
	{
	}
}
