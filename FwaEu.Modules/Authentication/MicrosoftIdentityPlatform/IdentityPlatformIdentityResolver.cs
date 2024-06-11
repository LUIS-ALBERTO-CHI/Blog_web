using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.MicrosoftIdentityPlatform
{
	public class IdentityPlatformIdentityResolver : IIdentityResolver
	{
		public bool CanHandleAuthenticationChangeInfoService => false;

		public string ResolveIdentity(ClaimsIdentity user)
		{
			return user.Claims.SingleOrDefault(claim =>
				claim.Type == "preferred_username")?.Value;
		}

		string IIdentityResolver.ResolveIdentity(ClaimsPrincipal user)
		{
			return this.ResolveIdentity((ClaimsIdentity)user.Identity);
		}
	}
}
