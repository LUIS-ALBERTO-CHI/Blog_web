using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public class EntityAuthenticationChangeInfoService : IAuthenticationChangeInfoService
	{
		public EntityAuthenticationChangeInfoService(
			MainSessionContext sessionContext,
			ICurrentDateTime currentDateTime)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this._currentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));
		}

		private readonly MainSessionContext _sessionContext;
		private readonly ICurrentDateTime _currentDateTime;

		public bool Enabled => true;

		public async Task<DateTime?> GetLastChangeDateAsync(int userId)
		{
			var repository = this._sessionContext.RepositorySession.Create<UserAuthenticationTokenEntityRepository>();

			var date = await repository.QueryByUserId(userId)
				.Select(u => u.AuthenticationInfoChangedOn)
				.WithOptions(option => option.SetCacheable(true).SetCacheRegion(UserEntity.CacheRegionName))
				.FirstOrDefaultAsync();

			return date;
		}

		public async Task SetLastChangeDateAsync(int userId)
		{
			var repository = this._sessionContext.RepositorySession.Create<UserAuthenticationTokenEntityRepository>();

			var entity = (await repository.QueryByUserId(userId)
				.FirstOrDefaultAsync()) ?? new UserAuthenticationTokenEntity()
				{
					User = await this._sessionContext.RepositorySession
						.Create<IUserEntityRepository>()
						.GetAsync(userId),
				};

			if (entity.User.Id != userId)
			{
				throw new NotSupportedException();
			}

			entity.AuthenticationInfoChangedOn = this._currentDateTime.Now;
			await repository.SaveOrUpdateAsync(entity);
		}
	}
}
