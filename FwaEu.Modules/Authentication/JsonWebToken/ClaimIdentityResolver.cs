using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public class ClaimIdentityResolver : IIdentityResolver
	{
		public bool CanHandleAuthenticationChangeInfoService => true;

		public string ResolveIdentity(ClaimsPrincipal user)
		{
			return ((ClaimsIdentity)user.Identity).Claims.SingleOrDefault(claim =>
				claim.Type == JsonWebTokenWriter.IdentityClaimType)?.Value;
		}
	}
}
