using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public class AuthenticatedUser
	{
		public AuthenticatedUser(string token)
		{
			this.Token = token;
		}

		public string Token { get; }
	}
}
