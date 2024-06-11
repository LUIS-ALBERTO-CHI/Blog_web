using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByRole
{
	public class UserRoleEntity : IEntity, IUpdateTracked
	{
		public int Id { get; protected set; }

		public UserEntity User { get; set; }
		public RoleEntity Role { get; set; }

		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserRoleEntityRepository : DefaultRepository<UserRoleEntity, int>
	{
		public IQueryable<UserRoleEntity> QueryByUserId(int userId)
		{
			return this.Query().Where(ur => ur.User.Id == userId);
		}

	}

	public class UserRoleEntityClassMap : ClassMap<UserRoleEntity>
	{
		public UserRoleEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_UserRole");
			References(entity => entity.Role).Not.Nullable().UniqueKey("UQ_UserRole");
			this.AddUpdateTrackedPropertiesIntoMapping();
		}
	}
}
