using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public enum CurrentUserLoadResult
	{
		NotAuthenticated,
		UnknownUser,
		AuthenticationInfoChanged,
		Loaded
	}

	public interface ICurrentUserService
	{
		Task<CurrentUserLoadResult> LoadAsync();
		ICurrentUser User { get; }
	}

	public class HttpContextIdentityCurrentUserService : ICurrentUserService
	{
		public HttpContextIdentityCurrentUserService(
			IHttpContextAccessor httpContextAccessor,
			MainSessionContext sessionContext,
			IAuthenticationChangeInfoService authenticationChangeInfoService,
			IClaimValueConvertService<DateTime, string> claimValueConvertService,
			IEnumerable<IIdentityResolver> identityResolvers,
			ILoggerFactory loggerFactory)
		{
			this._logger = loggerFactory.CreateLogger<HttpContextIdentityCurrentUserService>();

			this._httpContextAccessor = httpContextAccessor
				?? throw new ArgumentNullException(nameof(httpContextAccessor));

			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));

			this._claimValueConvertService = claimValueConvertService
				?? throw new ArgumentNullException(nameof(claimValueConvertService));

			if (identityResolvers.Count() == 0)
			{
				throw new ArgumentException("There must be at least one identity resolver.");
			}
			this._identityResolvers = identityResolvers;
		}

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly MainSessionContext _sessionContext;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;
		private readonly IClaimValueConvertService<DateTime, string> _claimValueConvertService;
		private readonly IEnumerable<IIdentityResolver> _identityResolvers;
		private readonly ILogger _logger;

		public ICurrentUser User { get; private set; }

		public async Task<CurrentUserLoadResult> LoadAsync()
		{
			var user = this._httpContextAccessor.HttpContext.User;

			if (user.Identity.IsAuthenticated)
			{
				var identity = this._identityResolvers.Select(ir => new { Identity = ir.ResolveIdentity(user),  ir.CanHandleAuthenticationChangeInfoService}).First(i => i.Identity != null);
				
			

				var authenticationInfoChangedOnClaim = default(Claim);

				if (identity.CanHandleAuthenticationChangeInfoService && this._authenticationChangeInfoService.Enabled)
				{
					authenticationInfoChangedOnClaim = ((ClaimsIdentity)user.Identity).Claims
						.SingleOrDefault(claim => claim.Type == AuthenticationClaimTypes.AuthenticationInfoChangedOn);

					if (authenticationInfoChangedOnClaim == null)
					{
						return CurrentUserLoadResult.AuthenticationInfoChanged;
					}
				}

				var entity = default(UserEntity);

				try
				{
					entity = await this._sessionContext.RepositorySession
						.Create<IUserEntityRepository>()
						.GetByIdentityAsync(identity.Identity);
				}
				catch (UserNotFoundException)
				{
					this._logger.LogInformation($"User '{identity.Identity}' authenticated but not found in database");
					return CurrentUserLoadResult.UnknownUser;
				}

				if (identity.CanHandleAuthenticationChangeInfoService && this._authenticationChangeInfoService.Enabled)
				{
					var tokenAuthenticationInfoDate = _claimValueConvertService.FromClaimValue(authenticationInfoChangedOnClaim.Value);
					var lastChangeDate = await this._authenticationChangeInfoService.GetLastChangeDateAsync(entity.Id);

					if (lastChangeDate != null)
					{
						//NOTE : We must remove milliseconds from DateTime since we compare it to an Unix timestamp converted DateTime 
						// and Unix timestamp don't have milliseconds.
						lastChangeDate = new DateTime(
							lastChangeDate.Value.Year,
							lastChangeDate.Value.Month,
							lastChangeDate.Value.Day,
							lastChangeDate.Value.Hour,
							lastChangeDate.Value.Minute,
							lastChangeDate.Value.Second);

						if (lastChangeDate > tokenAuthenticationInfoDate)
						{
							return CurrentUserLoadResult.AuthenticationInfoChanged;
						}
					}
				}

				this.User = new CurrentUser(entity.Identity, entity);
				return CurrentUserLoadResult.Loaded;
			}
			else
			{
				this.User = null;
				return CurrentUserLoadResult.NotAuthenticated;
			}
		}
	}
}
