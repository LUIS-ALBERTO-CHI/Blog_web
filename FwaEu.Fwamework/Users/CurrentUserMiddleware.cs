using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class CurrentUserMiddleware
	{
		private readonly RequestDelegate _next;

		public CurrentUserMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext,
			ICurrentUserService currentUserService, ILogger<CurrentUserMiddleware> logger)
		{
			var result = await currentUserService.LoadAsync();
			if (result == CurrentUserLoadResult.AuthenticationInfoChanged || result == CurrentUserLoadResult.UnknownUser)
			{
				httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await httpContext.Response.WriteAsync(result.ToString());
			}
			else
			{
				using (logger.BeginScope(new Dictionary<string, object>()
				{
					{"CurrentUser", currentUserService.User?.Identity },
				}))
				{
					await this._next(httpContext);
				}
			}
		}
	}

	public static class CurrentUserMiddlewareExtensions
	{
		public static IApplicationBuilder UseFwameworkCurrentUser(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CurrentUserMiddleware>();
		}
	}
}
