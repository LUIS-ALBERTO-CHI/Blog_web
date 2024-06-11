using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public class CurrentUserPerimeterMiddleware
	{
		private readonly RequestDelegate _next;

		public CurrentUserPerimeterMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext,
			ICurrentUserPerimeterService currentUserPerimeterService)
		{
			await currentUserPerimeterService.LoadFullAccessesAsync();
			await this._next(httpContext);
		}
	}

	public static class UserPerimetersMiddlewareExtensions
	{
		public static IApplicationBuilder UseFwameworkCurrentUserPerimeter(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CurrentUserPerimeterMiddleware>();
		}
	}
}
