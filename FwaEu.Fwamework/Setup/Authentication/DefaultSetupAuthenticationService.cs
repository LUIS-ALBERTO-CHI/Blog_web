using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.Authentication
{
	public class SetupAuthenticatedModel 
	{
		public SetupAuthenticatedModel(string token)
		{
			Token = token ?? throw new ArgumentNullException(nameof(token));
		}
		public string Token { get; set; }
	}

	public interface ISetupAuthenticationService
	{
		Task<SetupAuthenticatedModel> AuthenticateAsync(string identity, string password);
	}

	public class DefaultSetupAuthenticationService : ISetupAuthenticationService
	{
		private readonly SecuritySetupOptions _securitySetupOptions;
		private readonly ISetupJsonWebTokenWriter _setupJsonWebTokenWriter;

		public DefaultSetupAuthenticationService(IOptions<SetupOptions> setupOptions,
			ISetupJsonWebTokenWriter setupJsonWebTokenWriter)
		{
			_securitySetupOptions = setupOptions.Value?.Security;
			_setupJsonWebTokenWriter = setupJsonWebTokenWriter ?? throw new ArgumentNullException(nameof(setupJsonWebTokenWriter));
		}

		public async Task<SetupAuthenticatedModel> AuthenticateAsync(string login, string password)
		{
			if (this._securitySetupOptions != null //NOTE: In case server disabled setup authentication but client did not, token will still be created
				&& (!String.Equals(this._securitySetupOptions.Login, login, StringComparison.InvariantCultureIgnoreCase)
					|| this._securitySetupOptions.Password != password))
			{
				return null;
			}
			return new SetupAuthenticatedModel(
				await _setupJsonWebTokenWriter.WriteTokenAsync(login));
		}
	}
}
