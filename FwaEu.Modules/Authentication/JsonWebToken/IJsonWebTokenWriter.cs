using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public interface IJsonWebTokenWriter
	{
		Task<string> WriteTokenAsync(TokenInfo tokenInfo);
	}

	public class TokenInfo
	{
		public TokenInfo(int userId, string identity)
		{
			this.UserId = userId;
			this.Identity = identity ?? throw new ArgumentNullException(nameof(identity));
		}

		public int UserId { get; }
		public string Identity { get; }
	}

	public class JsonWebTokenWriter : IJsonWebTokenWriter
	{
		public const string IdentityClaimType = "Identity";

		public JsonWebTokenWriter(
			IOptions<JsonWebTokenOptions> settings,
			ICurrentDateTime currentDateTime,
			IAuthenticationChangeInfoService authenticationChangeInfoService,
			MainSessionContext sessionContext,
			IClaimValueConvertService<DateTime, string> claimValueConvertService)
		{
			_ = settings ?? throw new ArgumentNullException(nameof(settings));

			this._settings = settings.Value;

			this._currentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));

			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this._claimValueConvertService = claimValueConvertService
				?? throw new ArgumentNullException(nameof(claimValueConvertService));
		}

		private readonly JsonWebTokenOptions _settings;
		private readonly ICurrentDateTime _currentDateTime;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;
		private readonly MainSessionContext _sessionContext;
		private readonly IClaimValueConvertService<DateTime, string> _claimValueConvertService;

		public async Task<string> WriteTokenAsync(TokenInfo tokenInfo)
		{
			var now = this._currentDateTime.Now;
			var tokenHandler = new JwtSecurityTokenHandler();

			var claims = new List<Claim>(3);
			claims.Add(new Claim(IdentityClaimType,
				tokenInfo.Identity, ClaimValueTypes.String));

			if (this._authenticationChangeInfoService.Enabled)
			{
				var lastChangeDate = await this._authenticationChangeInfoService
					.GetLastChangeDateAsync(tokenInfo.UserId);

				claims.Add(new Claim(AuthenticationClaimTypes.AuthenticationInfoChangedOn,
					lastChangeDate != null ? _claimValueConvertService.ToClaimValue(lastChangeDate.Value)
						: _claimValueConvertService.ToClaimValue(now), ClaimValueTypes.Integer64));
			}

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Expires = now.AddDays(this._settings.ExpirationDelayInDays),

				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._settings.SigningKey)),
					SecurityAlgorithms.HmacSha256Signature),

				Subject = new ClaimsIdentity(claims.ToArray(), "JsonWebToken"),
			};

			var token = tokenHandler.WriteToken(
				tokenHandler.CreateToken(tokenDescriptor));

			var repository = this._sessionContext.RepositorySession.Create<UserAuthenticationTokenEntityRepository>();

			var entity = (await repository.QueryByUserId(tokenInfo.UserId).FirstOrDefaultAsync())
				?? new UserAuthenticationTokenEntity()
				{
					User = await this._sessionContext.RepositorySession.Create<IUserEntityRepository>()
						.GetAsync(tokenInfo.UserId),
				};

			entity.PreviousTokenGeneratedOn = now;
			await repository.SaveOrUpdateAsync(entity);

			return token;
		}
	}
}
