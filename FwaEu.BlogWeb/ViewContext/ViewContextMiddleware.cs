using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.ViewContext
{
	public class ViewContextMiddleware
	{
		private readonly RequestDelegate _next;

		public ViewContextMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext,
			IViewContextService viewContextService, ILogger<ViewContextMiddleware> logger)
		{
			var result = await viewContextService.LoadAsync();
			if (result == ViewContextLoadResult.OutOfPerimeter)
			{
				httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
				await httpContext.Response.WriteAsync(result.ToString());
			}
			else
			{
				using (logger.BeginScope(viewContextService.Current?.ToDictionary()))
				{
					await this._next(httpContext);
				}
			}
		}
	}

	public static class ViewContextMiddlewareExtensions
	{
		public static IApplicationBuilder UseApplicationViewContext(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ViewContextMiddleware>();
		}
	}
}
