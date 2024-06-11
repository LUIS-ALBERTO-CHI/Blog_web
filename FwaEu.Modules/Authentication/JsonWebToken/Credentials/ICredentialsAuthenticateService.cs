using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials
{
	public interface ICredentialsAuthenticateService
	{
		Task<AuthenticatedUser> AuthenticateAsync(string identity, string password);
	}
}
