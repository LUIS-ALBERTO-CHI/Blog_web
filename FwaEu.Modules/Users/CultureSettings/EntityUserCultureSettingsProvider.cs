using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.CultureSettings
{
	public class EntityUserCultureSettingsProvider : IUserCultureSettingsProvider
	{
		public EntityUserCultureSettingsProvider(ISessionAdapterFactory sessionAdapterFactory, IRepositoryFactory repositoryFactory)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;

		public async Task<ICultureSettings> GetCultureSettingsAsync(int userId)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var entity = await this._repositoryFactory.Create<UserCultureSettingsEntityRepository>(session)
					.GetByUserIdAsync(userId);

				return new CultureSettingsModel(entity.LanguageTwoLetterIsoCode);
			}
		}
	}
}
