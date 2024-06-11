using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public class JsonWebTokenRenewerService : ITokenRenewerService
	{
		public JsonWebTokenRenewerService(
			ICurrentUserService currentUserService,
			IJsonWebTokenWriter jsonWebTokenWriter,
			MainSessionContext sessionContext)
		{
			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));

			this._jsonWebTokenWriter = jsonWebTokenWriter
				?? throw new ArgumentNullException(nameof(jsonWebTokenWriter));

			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		private readonly ICurrentUserService _currentUserService;
		private readonly IJsonWebTokenWriter _jsonWebTokenWriter;
		private readonly MainSessionContext _sessionContext;

		public async Task<string> RenewTokenAsync()
		{
			var token = default(string);
			var user = this._currentUserService.User;

			token = await this._jsonWebTokenWriter.WriteTokenAsync(
				new TokenInfo(user.Entity.Id, user.Identity));

			await this._sessionContext.RepositorySession.Session.FlushAsync();

			return token;
		}
	}
}
