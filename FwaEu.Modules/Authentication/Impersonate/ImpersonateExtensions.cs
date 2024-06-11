using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Setup;
using FwaEu.Modules.Authentication.Impersonate.JsonWebToken;
using FwaEu.Modules.Authentication.Impersonate.Setup;
using FwaEu.Modules.Authentication.JsonWebToken;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.Impersonate
{
	public static class ImpersonateExtensions
	{
		public static readonly string SchemeName = "Impersonate";

		public static IServiceCollection AddImpersonateServices(this IServiceCollection services,
			ApplicationInitializationContext context, IAuthenticationFeatures authenticationFeatures)
		{
			var section = context.Configuration.GetSection("Fwamework:Authentication:JsonWebToken");

			services.AddTransient<IImpersonateService, JsonWebTokenImpersonateService>();

			services.AddTransient<ISetupTask, ImpersonateTask>();

			services.AddAuthentication(SchemeName)
				.AddJwtBearer(SchemeName, options => JsonWebTokenAuthenticationInitializer.ConfigureJwtBearerOptions(options, section, context));
			authenticationFeatures.AuthenticationSchemes.Add(SchemeName);

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserAuthenticationTokenEntityRepository>();

			var resolvedAuthenticationFeatures = authenticationFeatures as AuthenticationFeatures;
			if (resolvedAuthenticationFeatures != null)
			{
				resolvedAuthenticationFeatures.AddMappingType(typeof(UserAuthenticationTokenEntityClassMap));
			}
			return services;
		}
	}
}
