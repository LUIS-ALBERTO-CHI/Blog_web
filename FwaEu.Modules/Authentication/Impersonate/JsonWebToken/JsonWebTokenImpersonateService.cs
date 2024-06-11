using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Authentication.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.Impersonate.JsonWebToken
{
	public class JsonWebTokenImpersonateService : IImpersonateService
	{
		public JsonWebTokenImpersonateService(
			ISessionAdapterFactory sessionAdapterFactory,
			MainSessionContext sessionContext,
			IJsonWebTokenWriter jsonWebTokenWriter)
		{
			_sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			_sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			_jsonWebTokenWriter = jsonWebTokenWriter
				?? throw new ArgumentNullException(nameof(jsonWebTokenWriter));
		}

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly MainSessionContext _sessionContext;
		private readonly IJsonWebTokenWriter _jsonWebTokenWriter;

		public async Task<AuthenticatedUser> ImpersonateAsync(string identity)
		{
			var user = await _sessionContext.RepositorySession.Create<IUserEntityRepository>()
				.GetByIdentityAsync(identity);

			if (user == null)
			{
				return null;
			}

			var token = await _jsonWebTokenWriter.WriteTokenAsync(
				new TokenInfo(user.Id, user.Identity));

			await _sessionContext.RepositorySession.Session.FlushAsync();

			return new AuthenticatedUser(token);
		}
	}
}
