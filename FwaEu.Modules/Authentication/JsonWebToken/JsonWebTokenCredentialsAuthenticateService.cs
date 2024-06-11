using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public class JsonWebTokenCredentialsAuthenticateService : ICredentialsAuthenticateService
	{
		public JsonWebTokenCredentialsAuthenticateService(
			MainSessionContext sessionContext,
			IPasswordHasher passwordHasher,
			IJsonWebTokenWriter jsonWebTokenWriter)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this._passwordHasher = passwordHasher
				?? throw new ArgumentNullException(nameof(passwordHasher));

			this._jsonWebTokenWriter = jsonWebTokenWriter
				?? throw new ArgumentNullException(nameof(jsonWebTokenWriter));
		}

		private readonly MainSessionContext _sessionContext;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IJsonWebTokenWriter _jsonWebTokenWriter;

		public async Task<AuthenticatedUser> AuthenticateAsync(string identity, string password)
		{
			var hashedPassword = this._passwordHasher.Hash(password);

			var existingUser = await this._sessionContext
				.RepositorySession
				.Create<UserCredentialsEntityRepository>()
				.QueryByIdentityAndPassword(identity, hashedPassword)
				.Select(uc => new { Id = uc.User.Id, Identity = uc.User.Identity })
				.FirstOrDefaultAsync();

			if (existingUser != null)
			{
				var token = await this._jsonWebTokenWriter.WriteTokenAsync(
					new TokenInfo(existingUser.Id, existingUser.Identity));

				await this._sessionContext.RepositorySession.Session.FlushAsync();

				return new AuthenticatedUser(token);
			}

			return null;
		}
	}
}
