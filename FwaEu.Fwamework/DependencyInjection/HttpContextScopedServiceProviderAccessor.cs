using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Fwamework.DependencyInjection
{
    public class HttpContextScopedServiceProviderAccessor : IScopedServiceProviderAccessor
	{
		public HttpContextScopedServiceProviderAccessor(IHttpContextAccessor httpContextAccessor)
		{
			this._httpContextAccessor = httpContextAccessor;
		}

		private readonly IHttpContextAccessor _httpContextAccessor;

		public IServiceProvider GetScopeServiceProvider()
		{
			return this._httpContextAccessor.HttpContext?.RequestServices;
		}
	}
}
