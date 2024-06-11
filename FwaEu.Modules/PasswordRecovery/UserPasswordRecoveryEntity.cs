using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.PasswordRecovery
{
	public class UserPasswordRecoveryEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; protected set; }
		public UserEntity User { get; set; }
		public Guid? Guid { get; set; } 
		public DateTime UpdatedOn { get; set; }
		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserPasswordRecoveryEntityRepository : DefaultRepository<UserPasswordRecoveryEntity, int>
	{
		public async Task<UserPasswordRecoveryEntity> GetByUserIdAsync(int userId)
		{
			return await this.Query()
				.FirstOrDefaultAsync(uc => uc.User.Id == userId);
		}
	}
	public class UserPasswordRecoveryEntityClassMap : ClassMap<UserPasswordRecoveryEntity>
	{
		public UserPasswordRecoveryEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().Unique();
			Map(entity => entity.Guid).Nullable();
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}
}
