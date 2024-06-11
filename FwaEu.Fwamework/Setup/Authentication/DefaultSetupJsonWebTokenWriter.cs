using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.Authentication
{
	public interface ISetupJsonWebTokenWriter
	{
		Task<string> WriteTokenAsync(string identity);
	}

	public class DefaultSetupJsonWebTokenWriter : ISetupJsonWebTokenWriter
	{
		public const string IdentityClaimType = "Identity";
		public const string AuthenticatedOnClaimType = "AuthenticatedOn";

		private readonly SecuritySetupOptions _securitySetupOptions;
		private readonly ICurrentDateTime _currentDateTime;
		private readonly IClaimValueConvertService<DateTime, string> _dateTimeClaimValueConverterService;

		public DefaultSetupJsonWebTokenWriter(
			IOptionsMonitor<SetupOptions> setupOptions, ICurrentDateTime currentDateTime, 
			IClaimValueConvertService<DateTime, string> dateTimeClaimValueConverterService)
		{
			_currentDateTime = currentDateTime ?? throw new ArgumentNullException(nameof(currentDateTime));

			_dateTimeClaimValueConverterService = dateTimeClaimValueConverterService
				?? throw new ArgumentNullException(nameof(dateTimeClaimValueConverterService));

			_securitySetupOptions = setupOptions.CurrentValue.Security;
		}

		public Task<string> WriteTokenAsync(string identity)
		{
			var now = this._currentDateTime.Now;
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Expires = this._securitySetupOptions != null 
				? now.AddMinutes(this._securitySetupOptions.ExpirationDelayInMinutes)
				: now,

				SigningCredentials = this._securitySetupOptions != null
					? new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._securitySetupOptions.TokenSigningKey)),
						SecurityAlgorithms.HmacSha256Signature)
					: null,

				Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(IdentityClaimType, identity, ClaimValueTypes.String),
						new Claim(AuthenticatedOnClaimType, 
							this._dateTimeClaimValueConverterService.ToClaimValue(now), ClaimValueTypes.Integer64),
					},
					SetupSecurityOptions.SchemeName),
			};

			return Task.FromResult(tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)));
		}
	}
}
