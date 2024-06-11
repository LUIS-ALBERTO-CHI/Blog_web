using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByUser
{
	public class UserPermissionEntity : IEntity
	{
		public int Id { get; set; }

		public UserEntity User { get; set; }
		public PermissionEntity Permission { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserPermissionEntityRepository : DefaultRepository<UserPermissionEntity, int>
	{
		public IQueryable<UserPermissionEntity> QueryByUserId(int userId)
		{
			return this.Query().Where(up => up.User.Id == userId);
		}
	}

	public class UserPermissionEntityClassMap : ClassMap<UserPermissionEntity>
	{
		public UserPermissionEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();

			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_UserPermission");
			References(entity => entity.Permission).Not.Nullable().UniqueKey("UQ_UserPermission");
		}
	}
}
