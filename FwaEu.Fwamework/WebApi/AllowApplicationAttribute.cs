using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;

namespace FwaEu.Fwamework.WebApi
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class AllowApplicationAttribute : TypeFilterAttribute
	{
		public AllowApplicationAttribute(params string[] applicationNames) : base(typeof(AllowApplicationFilter))
		{
			Arguments = new object[] { applicationNames };
		}
	}

	public class AllowApplicationFilter : IAuthorizationFilter
	{
		private readonly string[] _applicationNames;

		private IApplicationAllowedCheckService _applicationAllowedCheckService;
		private AllowedWebApiOptions _webApiSettings;

		public AllowApplicationFilter(string[] applicationNames, 
			IApplicationAllowedCheckService applicationAllowedCheckService,
			IOptions<AllowedWebApiOptions> settings)
		{
			_applicationNames = applicationNames;
			_ = settings.Value ?? throw new ArgumentException("Value is null.", nameof(settings));
			_webApiSettings = settings.Value;
			_applicationAllowedCheckService = applicationAllowedCheckService ?? throw new ArgumentNullException(nameof(applicationAllowedCheckService));
		}

		private bool IsRequestAccepted(AuthorizationFilterContext actionContext, string[] applicationNames)
		{
			var header = actionContext.HttpContext.Request.Headers.FirstOrDefault(h => _webApiSettings.HeaderName.Equals(h.Key,  StringComparison.InvariantCultureIgnoreCase));
			var secret = header.Value.FirstOrDefault();
			var ip = actionContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

			// If matching header is missing in API call: authorization not granted 
			if (secret == null)
				return false;

			var methodArgument = (actionContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo
				.GetCustomAttributes<AllowApplicationAttribute>().FirstOrDefault();

			// If the method has an attribute, it should overrides the class attribute
			if (methodArgument != null)
			{
				applicationNames = Array.ConvertAll((object[])methodArgument.Arguments?.FirstOrDefault(), Convert.ToString);
			}

			// If the attribute exists but with no parameters, access is granted if secret matches any application and if ip is allowed
			if (!applicationNames.Any())
				return _applicationAllowedCheckService.IsSecretMatchWithAnyApplication(secret, ip); 

			// If the application name and secret matches and ip is allowed, authorization is granted 
			foreach (string name in applicationNames)
			{
				if (_applicationAllowedCheckService.IsApplicationAllowed(name, secret, ip))
					return true;
			}
			return false;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			bool isAuthorized = IsRequestAccepted(context, _applicationNames);

			if (!isAuthorized)
			{
				context.Result = new UnauthorizedResult();
			}
		}
	}
}
