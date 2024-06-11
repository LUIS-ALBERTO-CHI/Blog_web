using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public class UserAuthenticationTokenEntity : IEntity
	{
		public int Id { get; protected set; }
		public UserEntity User { get; set; }

		public DateTime? AuthenticationInfoChangedOn { get; set; }
		public DateTime? PreviousTokenGeneratedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserAuthenticationTokenEntityRepository : DefaultRepository<UserAuthenticationTokenEntity, int>
	{
		public IQueryable<UserAuthenticationTokenEntity> QueryByUserId(int userId)
		{
			return this.Query()
				.Where(uc => uc.User.Id == userId)
				.WithOptions(options => options
					.SetCacheable(true)
					.SetCacheRegion(UserEntity.CacheRegionName));
		}
	}

	public class UserAuthenticationTokenEntityClassMap : ClassMap<UserAuthenticationTokenEntity>, IAuthenticationClassMap
	{
		public UserAuthenticationTokenEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().Unique();
			Map(entity => entity.AuthenticationInfoChangedOn).Nullable();
			Map(entity => entity.PreviousTokenGeneratedOn).Nullable();
		}
	}
}
