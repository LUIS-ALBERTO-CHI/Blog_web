using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByUser
{
	public class ByUserPermissionManager : IPermissionManager
	{
		public ByUserPermissionManager(
			ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory)
		{
			this._sessionAdapterFactory = sessionAdapterFactory;
			this._repositoryFactory = repositoryFactory;
		}

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;

		public async Task<bool> HasPermissionAsync(UserEntity user, IPermission permission)
		{
			if (user == null || user.IsAdmin)
			{
				return (user?.IsAdmin).GetValueOrDefault();
			}

			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var data = await this._repositoryFactory.Create<UserPermissionEntityRepository>(session)
					.Query()
					.Where(up => up.User.Id == user.Id) //NOTE: Query Cache is not working if using up.User == user
					.Select(up => new
					{
						PermissionInvariantId = up.Permission.InvariantId,
					})
					.WithOptions(options => options
						.SetCacheable(true)
						.SetCacheRegion(UserEntity.CacheRegionName))
					.ToListAsync();

				return data.Any(p => p.PermissionInvariantId == permission.InvariantId);
			}
		}
	}
}