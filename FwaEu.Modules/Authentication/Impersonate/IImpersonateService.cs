using FwaEu.Modules.Authentication.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.Impersonate
{
	public interface IImpersonateService
	{
		Task<AuthenticatedUser> ImpersonateAsync(string identity);
	}
}
