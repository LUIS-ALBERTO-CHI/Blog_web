using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public class PermissionEntity : IPermission, IEntity, IUpdatedOnTracked
	{
		protected PermissionEntity() // Constructor for loading from database
		{
		}

		public PermissionEntity(string invariantId)
		{
			this.InvariantId = invariantId;
		}

		public int Id { get; protected set; }
		public string InvariantId { get; protected set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class PermissionEntityRepository : DefaultRepository<PermissionEntity, int>, IQueryByIds<PermissionEntity, int>
	{
		public async Task<PermissionEntity> GetByInvariantIdAsync(string invariantId)
		{
			return await this.Query()
				.Where(p => p.InvariantId == invariantId)
				.SingleOrDefaultAsync();
		}

		public IQueryable<PermissionEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
	}

	public class PermissionEntityClassMap : ClassMap<PermissionEntity>
	{
		public PermissionEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.InvariantId).Not.Nullable().Unique();
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}
}
