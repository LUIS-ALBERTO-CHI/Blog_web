using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface IIdentityResolver
	{
		string ResolveIdentity(ClaimsPrincipal user);
		bool CanHandleAuthenticationChangeInfoService { get;}

	}
}
