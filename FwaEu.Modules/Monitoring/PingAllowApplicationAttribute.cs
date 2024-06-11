using FwaEu.Fwamework.WebApi;
using FwaEu.Modules.Monitoring.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Monitoring
{
	public class PingAllowApplicationAttribute : TypeFilterAttribute
	{
		public PingAllowApplicationAttribute() : base(typeof(PingAllowApplicationFilter))
		{

		}
	}

	public class PingAllowApplicationFilter : IAuthorizationFilter
	{
		private PingAllowApplicationOptions _settings;
		private IApplicationAllowedCheckService _applicationAllowedCheckService;
		private IOptions<AllowedWebApiOptions> _webApiSettings;

		public PingAllowApplicationFilter(IOptions<PingAllowApplicationOptions> settings,
			IApplicationAllowedCheckService applicationAllowedCheckService, IOptions<AllowedWebApiOptions> webApiSettings)
		{
			_settings = settings.Value;
			_webApiSettings = webApiSettings;
			_applicationAllowedCheckService = applicationAllowedCheckService ?? throw new ArgumentNullException(nameof(applicationAllowedCheckService));
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!_settings.Enabled)
				return;

			var filter = new AllowApplicationFilter(new string[0], _applicationAllowedCheckService, _webApiSettings);
			filter.OnAuthorization(context);
		}
	}
}

