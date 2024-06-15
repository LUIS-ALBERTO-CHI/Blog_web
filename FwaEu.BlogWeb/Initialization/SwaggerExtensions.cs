using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.BlogWeb.Initialization
{
	public static class SwaggerExtensions
	{
		private static SwaggerOptions GetSettings(IConfiguration configuration)
		{ 
			return configuration.GetSection("Application:Swagger").Get<SwaggerOptions>();
		}

		public static IServiceCollection AddApplicationSwagger(this IServiceCollection services,
			IAuthenticationFeatures authenticationFeatures,
			ApplicationInitializationContext context)
		{
			if (GetSettings(context.Configuration).Enabled)
			{
				services.AddSwaggerGen(options =>
				{
					options.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();

					options.EnableAnnotations();

					options.SwaggerDoc("v1.0", new OpenApiInfo { Title = "BlogWeb v1.0", Version = "v1.0" });

					if (authenticationFeatures.UseBearer)
					{
						options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
						{
							Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
							Name = "Authorization",
							Scheme = "Bearer",
							In = ParameterLocation.Header,
							Type = SecuritySchemeType.ApiKey,
						});

						options.AddSecurityRequirement(new OpenApiSecurityRequirement()
						{
						{
							new OpenApiSecurityScheme()
							{
								Reference = new OpenApiReference()
								{
									Id = "Bearer",
									Type = ReferenceType.SecurityScheme,
								},
							},
							new List<string>()
						}
						});
					}
				});
			}

			return services;
		}

		public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder application, IConfiguration configuration)
		{
			if (GetSettings(configuration).Enabled)
			{
				application.UseSwagger();
				application.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("v1.0/swagger.json", "Versioned API v1.0");
					options.DocExpansion(DocExpansion.None);
				});

				//NOTE: Register a Middleware to redirect the root path to Swagger page
				application.Use(async (context, next) =>
				{
					if (context.Request.Path == "/")
					{
						context.Response.Redirect("./swagger/index.html", permanent: true);
						return;
					}

					await next();
				});
			}

			return application;
		}
	}
}
