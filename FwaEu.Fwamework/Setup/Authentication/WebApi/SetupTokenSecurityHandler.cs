using FwaEu.Fwamework.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.Authentication.WebApi
{
	public class SetupTokenSecurityHandler : AuthenticationHandler<SetupTokenAuthenticationSchemeOptions>
	{
		public const string AuthorizationHeaderKey = SetupSecurityHandler.SetupAuthorizationHeaderKey;
		public const string IdentityClaimType = "Identity";
		public const string SetupApplicationName = "Setup";

		private readonly IApplicationAllowedCheckService _applicationAllowedCheckService;
		private readonly ILogger _logger;

		public SetupTokenSecurityHandler(IOptionsMonitor<SetupTokenAuthenticationSchemeOptions> options,
			IApplicationAllowedCheckService applicationAllowedCheckService,
			ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
			: base(options, loggerFactory, encoder, clock)
		{
			_applicationAllowedCheckService = applicationAllowedCheckService ?? throw new ArgumentNullException(nameof(applicationAllowedCheckService));
			_logger = loggerFactory.CreateLogger<ApplicationAllowedCheckService>();
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey(AuthorizationHeaderKey))
			{
				return Task.FromResult(AuthenticateResult.Fail($"Missing {AuthorizationHeaderKey} Header."));
			}

			Request.Headers.TryGetValue(AuthorizationHeaderKey, out var secret);
			if (string.IsNullOrEmpty(secret))
			{
				return Task.FromResult(AuthenticateResult.Fail("Missing secret."));
			}

			var ip = Context.Connection.RemoteIpAddress.MapToIPv4().ToString();

			_logger.LogDebug($"Secret: '{secret}', ip: '{ip}'");
			if (!_applicationAllowedCheckService.IsApplicationAllowed(SetupApplicationName, secret, ip))
			{
				return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
			}

			var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim(IdentityClaimType, secret, ClaimValueTypes.String)
				}, Options.Scheme));
			var ticket = new AuthenticationTicket(principal, Options.Scheme);

			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
	}
}
