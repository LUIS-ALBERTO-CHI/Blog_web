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

namespace FwaEu.Fwamework.Setup.Authentication
{
	public class SetupSecurityHandler : AuthenticationHandler<SetupSecurityOptions>
	{
		public const string SetupAuthorizationHeaderKey = "Authorization";

		private readonly string _tokenSigningKey;
		private readonly bool _isSecurityEnabled;
		private readonly ILogger _logger;

		public SetupSecurityHandler(IOptionsMonitor<SetupSecurityOptions> options,
			IOptionsMonitor<SetupOptions> setupSettings,
			ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
			: base(options, loggerFactory, encoder, clock)
		{
			var setupOptions = setupSettings.CurrentValue;
			if (_isSecurityEnabled = setupOptions?.Security != null)
			{
				this._tokenSigningKey = setupOptions.Security.TokenSigningKey;
			}

			_logger = loggerFactory.CreateLogger<SetupSecurityHandler>();
		}

		private string ScrubToken(string rawToken)
		{
			return rawToken.Replace("Bearer ", "");
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			ClaimsPrincipal principal;

			if (!_isSecurityEnabled)
			{
				var claims = Array.Empty<Claim>();

				principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Options.Scheme));
			}
			else
			{
				if (!Request.Headers.ContainsKey(SetupAuthorizationHeaderKey))
				{
					return Task.FromResult(AuthenticateResult.Fail($"Missing {SetupAuthorizationHeaderKey} Header."));
				}
				Request.Headers.TryGetValue(SetupAuthorizationHeaderKey, out var rawToken);

				if (string.IsNullOrEmpty(rawToken))
				{
					return Task.FromResult(AuthenticateResult.Fail("Missing token."));
				}

				var token = ScrubToken(rawToken);
				var tokenHandler = new JwtSecurityTokenHandler();

				try
				{
					principal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = false,
						ValidateAudience = false,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(this._tokenSigningKey)),
					}, out _);

				}
				catch (Exception e)
				{
					_logger.LogDebug(e.Message, e);
					return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
				}
			}
			var ticket = new AuthenticationTicket(principal, Options.Scheme);

			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
	}
}
