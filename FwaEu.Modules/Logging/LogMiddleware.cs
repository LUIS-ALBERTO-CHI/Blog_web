using FwaEu.Fwamework.Application;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Logging
{
	public class LogMiddleware
	{
		private readonly RequestDelegate _next;

		public LogMiddleware(RequestDelegate next)
		{
			this._next = next ?? throw new ArgumentNullException(nameof(next));
		}

		private Dictionary<string, object> FillDefaultLoggerValues(
			HttpContext httpContext, ICurrentDateTime currentDateTime)
		{
			var properties = new Dictionary<string, object>
			{
				["Url"] = httpContext.Request.Path,
				["Time"] = currentDateTime.Now,
				["UserAgent"] = httpContext.Request.Headers["User-Agent"].ToString(),
			};
			return properties;
		}

		private async Task<Dictionary<string, object>> CreateExceptionContextScopeAsync(
			HttpContext httpContext, Exception exception, ICurrentUserService currentUserService)
		{
			var scope = new Dictionary<string, object>()
			{
				["CurrentUser"] = currentUserService.User?.Identity,
			};

			if (httpContext.Response?.StatusCode != StatusCodes.Status401Unauthorized)
				scope.Add("Body", await GetRequestBodyAsync(httpContext.Request));

			foreach (var key in exception.Data.Keys)
			{
				scope.Add($"Exception.{(string)key}", exception.Data[key]);
			}

			return scope;
		}

		private async Task<string> GetRequestBodyAsync(HttpRequest request)
		{
			// NOTE: Reset the position because it was already read by a later Middleware (WebApi...)
			request.Body.Position = 0;

			var body = await new StreamReader(request.Body).ReadToEndAsync();

			// NOTE: Reset the position in case of a previous Middleware needs it
			request.Body.Position = 0;

			return body;
		}

		public async Task Invoke(HttpContext httpContext, ILoggerFactory loggerFactory,
			ICurrentDateTime currentDateTime, ILogger<LogMiddleware> logger,
			ICurrentUserService currentUserService, IServiceProvider serviceProvider)
		{
			using (Serilog.Context.LogContext.Push(new LogEventEnricher(serviceProvider)))
			{
				using (logger.BeginScope(FillDefaultLoggerValues(httpContext, currentDateTime)))
				{
					try
					{
						httpContext.Request.EnableBuffering(); // NOTE: Enable buffering to allow read the request body multiple times.
						await _next(httpContext);
					}
					catch (Exception ex)
					{
						using ((logger.BeginScope(await this.CreateExceptionContextScopeAsync(httpContext, ex, currentUserService))))
						{
							loggerFactory.CreateLogger("UnhandledException")
								.LogCritical(ex, "Unhandled exception catch in LogMiddleware");
						}
						throw;
					}

					var statusCode = httpContext.Response?.StatusCode;
					if (statusCode >= StatusCodes.Status400BadRequest)
					{
						var loggerScopes = new Dictionary<string, object>
						{
							["StatusCode"] = statusCode,
							["CurrentUser"] = currentUserService.User?.Identity,
						};

						if (statusCode != StatusCodes.Status401Unauthorized)
							loggerScopes.Add("Body", await GetRequestBodyAsync(httpContext.Request));

						using (var loggerScope = logger.BeginScope(loggerScopes))
						{
							var statusCodeErrorLogger = loggerFactory.CreateLogger("StatusCodeError");
							if (statusCode < StatusCodes.Status500InternalServerError)
							{
								if (statusCode == StatusCodes.Status401Unauthorized)
								{
									statusCodeErrorLogger.LogInformation("A WebApi action returned an HTTP error");
								}
								else
								{
									statusCodeErrorLogger.LogWarning("A WebApi action returned an HTTP error");
								}
							}
							else
							{
								statusCodeErrorLogger.LogError("A WebApi action returned an HTTP error");
							}
						}
					}
				}
			}
		}
	}

	public static class LogMiddlewareExtensions
	{
		public static IApplicationBuilder UseFwameworkModuleLog(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<LogMiddleware>();
		}
	}
}

