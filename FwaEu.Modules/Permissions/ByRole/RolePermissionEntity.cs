using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByRole
{
	public class RolePermissionEntity : IEntity, IUpdateTracked
	{
		public int Id { get; protected set; }

		public RoleEntity Role { get; set; }
		public PermissionEntity Permission { get; set; }

		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class RolePermissionQueryByIdModel
	{
		public int RoleId { get; set; }
		public string PermissionInvariantId { get; set; }
	}

	public class RolePermissionEntityRepository : DefaultRepository<RolePermissionEntity, int>, IQueryByIds<RolePermissionEntity, RolePermissionQueryByIdModel>
	{
		public IQueryable<RolePermissionEntity> QueryByIds(RolePermissionQueryByIdModel[] ids)
		{
			//NOTE: Create a task if one day we need to implements QueryByIds() on this repository. No use case for the moment.
			throw new NotImplementedException();
		}
	}

	public class RolePermissionEntityClassMap : ClassMap<RolePermissionEntity>
	{
		public RolePermissionEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.Role).Not.Nullable().UniqueKey("UQ_RolePermission");
			References(entity => entity.Permission).Not.Nullable().UniqueKey("UQ_RolePermission");
			this.AddUpdateTrackedPropertiesIntoMapping();
		}
	}
}
