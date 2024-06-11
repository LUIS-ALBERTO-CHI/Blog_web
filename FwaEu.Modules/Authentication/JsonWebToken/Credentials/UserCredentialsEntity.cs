using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials
{
	public class UserCredentialsEntity : IEntity
	{
		public int Id { get; protected set; }
		public UserEntity User { get; set; }
		public string PasswordHash { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserCredentialsEntityRepository : DefaultRepository<UserCredentialsEntity, int>
	{
		public IQueryable<UserCredentialsEntity> QueryByIdentityAndPassword(string identity, string passwordHash)
		{
			return this.Query()
				.Where(uc => uc.User.Identity == identity
					&& uc.PasswordHash == passwordHash
					&& uc.User.State >= UserState.Active);
		}

		public async Task<UserCredentialsEntity> GetByUserIdAsync(int id)
		{
			return await this.Query()
				.FirstOrDefaultAsync(uc => uc.User.Id == id);
		}
	}

	public class UserCredentialsEntityClassMap : ClassMap<UserCredentialsEntity>, IAuthenticationClassMap
	{
		public UserCredentialsEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().Unique();
			Map(entity => entity.PasswordHash).Not.Nullable();
		}
	}
}
