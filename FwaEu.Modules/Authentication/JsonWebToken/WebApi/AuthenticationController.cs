using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Authentication;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FwaEu.Modules.Authentication.JsonWebToken.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		private IActionResult FeatureNotImplementedResult()
		{
			return StatusCode(StatusCodes.Status501NotImplemented);
		}

		[AllowAnonymous]
		[HttpPost(nameof(Authenticate))]
		[ProducesResponseType(typeof(AuthenticationResponseApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status501NotImplemented)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Authenticate(
			[FromBody] CredentialsApiModel model,
			[FromServices] IAuthenticationFeatures features,
			[FromServices] ICredentialsAuthenticateService credentialsAuthenticateService)
		{
			if (!features.EnableCredentialsAuthenticateAction)
			{
				return FeatureNotImplementedResult();
			}

			var user = await credentialsAuthenticateService.AuthenticateAsync(model.Identity, model.Password);
			if (user == null)
			{
				return Unauthorized("Invalid credentials.");
			}

			return Ok(new AuthenticationResponseApiModel
			{
				Token = user.Token,
			});
		}

		[HttpPost(nameof(RenewToken))]
		[ProducesResponseType(typeof(AuthenticationResponseApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status501NotImplemented)]
		public async Task<IActionResult> RenewToken(
			[FromServices]IAuthenticationFeatures features,
			[FromServices]ITokenRenewerService tokenRenewerService)
		{
			if (!features.EnableTokenRenewAction)
			{
				return this.FeatureNotImplementedResult();
			}

			var token = await tokenRenewerService.RenewTokenAsync();

			return Ok(new AuthenticationResponseApiModel
			{
				Token = token,
			});
		}
	}
}