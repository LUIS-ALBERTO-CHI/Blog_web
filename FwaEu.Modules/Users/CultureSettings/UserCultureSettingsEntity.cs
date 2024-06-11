using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.CultureSettings
{
	public class UserCultureSettingsEntity : IEntity
	{
		public int Id { get; protected set; }
		public UserEntity User { get; set; }
		public string LanguageTwoLetterIsoCode { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserCultureSettingsEntityRepository : DefaultRepository<UserCultureSettingsEntity, int>
	{
		public async Task<UserCultureSettingsEntity> GetByUserIdAsync(int userId)
		{
			return await this.Query()
				.FirstOrDefaultAsync(uc => uc.User.Id == userId);
		}
	}

	public class UserCultureSettingsEntityClassMap : ClassMap<UserCultureSettingsEntity>
	{
		public UserCultureSettingsEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().Unique();
			Map(entity => entity.LanguageTwoLetterIsoCode).Not.Nullable();
		}
	}
}
