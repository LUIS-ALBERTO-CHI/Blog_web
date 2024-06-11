using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class DefaultUsersByIdentityCache : IUsersByIdentityCache
	{
		public async Task<IEnumerable<UserByIdentityCacheModel>> LoadAsync(IQueryable<UserEntity> query)
		{
			return await query.Select(u => new UserByIdentityCacheModel()
			{
				UserId = u.Id,
				Identity = u.Identity,
			})
				.WithOptions(options => options
					.SetCacheable(true)
					.SetCacheRegion(UserEntity.CacheRegionName))
				.ToListAsync();
		}
	}
}
