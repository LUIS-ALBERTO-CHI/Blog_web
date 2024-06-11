using FwaEu.Fwamework.Data.Database;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Fwamework.Users
{
	public interface IUserEntityRepository : IRepository<UserEntity, int>
	{
		Task<UserEntity> FindByIdentityAsync(string identity);
		Task<UserEntity> GetByIdentityAsync(string identity);
		Task<bool> IsAdminAsync(int userId);
	}

	public abstract class DefaultUserEntityRepository<TUserEntity> : DefaultRepository<TUserEntity, int>, IUserEntityRepository
		where TUserEntity : UserEntity
	{
		public async Task<UserEntity> FindByIdentityAsync(string identity)
		{
			var cacheEntry = (await this.ServiceProvider.GetService<IUsersByIdentityCache>()
				.LoadAsync(this.Query()))
				.FirstOrDefault(ui => ui.Identity.Equals(identity, StringComparison.InvariantCultureIgnoreCase));

			if (cacheEntry == null)
			{
				return null;
			}

			return await this.GetAsync(cacheEntry.UserId);
		}

		public async Task<UserEntity> GetByIdentityAsync(string identity)
		{
			var user = await this.FindByIdentityAsync(identity);
			if (user == null)
			{
				throw new UserNotFoundException($"User not found with identity {identity}.");
			}

			return user;
		}

		public async Task<bool> IsAdminAsync(int userId)
		{
			return await this.QueryNoPerimeter()
				.WithOptions(options => options
					.SetCacheable(true)
					.SetCacheRegion(UserEntity.CacheRegionName))
				.AnyAsync(u => u.Id == userId && u.IsAdmin);
		}

		Task IRepository<UserEntity>.DeleteAsync(UserEntity entity)
		{
			return this.DeleteAsync((TUserEntity)entity);
		}

		Task IRepository<UserEntity>.SaveOrUpdateAsync(UserEntity entity)
		{
			return this.SaveOrUpdateAsync((TUserEntity)entity);
		}

		UserEntity IRepository<UserEntity>.CreateNew()
		{
			return this.CreateNew();
		}

		IQueryable<UserEntity> IRepository<UserEntity>.Query()
		{
			return this.Query();
		}

		IQueryable<UserEntity> IRepository<UserEntity>.QueryNoPerimeter()
		{
			return this.QueryNoPerimeter();
		}

		async Task<UserEntity> IRepository<UserEntity, int>.GetAsync(int id)
		{
			return await this.GetAsync(id);
		}

		async Task<UserEntity> IRepository<UserEntity, int>.GetNoPerimeterAsync(int id)
		{
			return await this.GetNoPerimeterAsync(id);
		}

		public virtual IQueryable<TUserEntity> QueryForUsersAdmin()
		{
			return this.Query();
		}
	}
}
