using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials.Parts;
using FwaEu.Modules.Authentication.JwtBearerEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public class JsonWebTokenAuthenticationInitializer : IAuthenticationInitializer
	{
		protected virtual void AddPasswordHasher(IServiceCollection services, IConfiguration configuration)
		{
			var section = configuration.GetSection("Fwamework:Authentication:Sha256PasswordHasher");
			services.Configure<Sha256PasswordHasherOptions>(section);
			services.AddTransient<IPasswordHasher, Sha256PasswordHasher>();
		}


		protected virtual void AddCredentialsServices(IServiceCollection services)
		{
			services.AddTransient<ICredentialsAuthenticateService, JsonWebTokenCredentialsAuthenticateService>();

			services.AddTransient<IChangePasswordCredentialsService, DefaultChangePasswordCredentialsService>();
			services.AddTransient<IPartHandler, CredentialsPartHandler>();
		}

		public static void ConfigureJwtBearerOptions(JwtBearerOptions options,
			IConfigurationSection section, ApplicationInitializationContext context)
		{
			var jsonWebTokenOptions = section.Get<JsonWebTokenOptions>();

			options.RequireHttpsMetadata = !context.Environment.IsDevelopment();
			options.SaveToken = true;
			options.MetadataAddress = jsonWebTokenOptions.MetadataAddress;

			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jsonWebTokenOptions.SigningKey)),
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
		}

		public IAuthenticationFeatures AddAuthentication(IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:Authentication:JsonWebToken");

			this.AddCredentialsServices(services);
			this.AddPasswordHasher(services, context.Configuration);

			services.AddTransient<IAuthenticationChangeInfoService, EntityAuthenticationChangeInfoService>();

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options => JsonWebTokenAuthenticationInitializer.ConfigureJwtBearerOptions(options, section, context));

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserCredentialsEntityRepository>();

			return new AuthenticationFeatures(
				enableCredentialsAuthenticateAction: true,
				enableTokenRenewAction: true,
				useBearer: true,
				mappingTypes: new List<Type> {
					typeof(UserCredentialsEntityClassMap),
				},
				authenticationSchemes: new List<string>()
				{
					JwtBearerDefaults.AuthenticationScheme
				}
			);
		}
	}
}
