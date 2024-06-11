using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Logging.WebApi
{
	public class RouteLogEnricherProvider : ILogEnricherProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RouteLogEnricherProvider(IHttpContextAccessor httpContextAccessor)
		{
			this._httpContextAccessor = httpContextAccessor
				?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public IEnumerable<LogEnricherProperty> GetProperties()
		{
			if (this._httpContextAccessor.HttpContext?.Request?.RouteValues
				is RouteValueDictionary values
				&& values.Count > 0)
			{
				yield return new LogEnricherProperty("Route",
					values.ToDictionary(kv => CamelToPascal(kv.Key), kv => kv.Value),
					true);
			}
		}
		private static string CamelToPascal(string value)
		{
			return value.Substring(0, 1).ToUpper() + value.Substring(1);
		}
	}
}
