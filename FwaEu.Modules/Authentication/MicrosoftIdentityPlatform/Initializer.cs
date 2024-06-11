using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Authentication.JwtBearerEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.MicrosoftIdentityPlatform
{
	public class Initializer : IAuthenticationInitializer
	{
		public IAuthenticationFeatures AddAuthentication(
			IServiceCollection services,
			ApplicationInitializationContext context)
		{
			services
				.AddAuthentication("Azure")
				.AddJwtBearer("Azure",options =>
				{
					var microsoftIdentityPlatformOptions = context.Configuration
						.GetSection("Fwamework:Authentication:MicrosoftIdentityPlatform")
						.Get<Options>();

					options.RequireHttpsMetadata = !context.Environment.IsDevelopment();
					options.SaveToken = true;
					options.MetadataAddress = microsoftIdentityPlatformOptions.MetadataAddress;

					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = false,
						ValidateAudience = false,
					};

					options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
					{
						OnMessageReceived = async context =>
						{
							foreach (var handler in context.HttpContext.RequestServices.GetServices<IJwtBearerEventsHandler>())
							{
								await handler.OnMessageReceivedAsync(context);
							}
						},
					};
				});

			services.AddTransient<IIdentityResolver, IdentityPlatformIdentityResolver>();

			return new AuthenticationFeatures(false, false, true, new List<Type>(), new List<string>() { "Azure" });
		}
	}
}
