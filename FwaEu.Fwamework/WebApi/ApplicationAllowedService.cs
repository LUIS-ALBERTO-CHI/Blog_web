using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public class ApplicationAllowedCheckService : IApplicationAllowedCheckService
	{
		private readonly IOptions<AllowedApplicationsOptions> _allowedApplicationsOptions;

		public ApplicationAllowedCheckService(IOptions<AllowedApplicationsOptions> allowedApplicationsOptions)
		{
			this._allowedApplicationsOptions = allowedApplicationsOptions
				?? throw new ArgumentNullException(nameof(allowedApplicationsOptions));
		}

		public bool IsApplicationAllowed(string applicationName, string secret, string ip)
		{
			return _allowedApplicationsOptions.Value.Applications
				.Any(x => x.Key == applicationName && x.Value.Secret == secret && IsIpAllowed(x.Value.Filter?.Allowed.ToArray(), ip));
		}

		public bool IsIpAllowed(string[] allowedIp, string hostIp)
		{
			// If no IP specified in settings, there is no IP filter 
			if (allowedIp == null)
				return true;
			return allowedIp.Contains(hostIp);
		}
		public bool IsSecretMatchWithAnyApplication(string secret, string ip)
		{
			return _allowedApplicationsOptions.Value.Applications
				.Any(x => x.Value.Secret == secret && IsIpAllowed(x.Value.Filter?.Allowed.ToArray(), ip));
		}
	}
}
