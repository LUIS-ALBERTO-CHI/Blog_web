using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public static class AuthenticationExtensions
	{
		public static IAuthenticationFeatures AddFwameworkAuthentication(
			this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:Authentication:Settings");
			var subSections = context.Configuration.GetSection("Fwamework:Authentication:Settings").Get<List<AuthSettings>>();

			services.Configure<AuthenticationOptions>(section);

			IAuthenticationFeatures authenticationFeatures = null;

			foreach (var subSection in subSections)
			{
				var initializer = (IAuthenticationInitializer)Activator.CreateInstance(Type.GetType(subSection.InitializerType));
				var features = initializer.AddAuthentication(services, context);
				if(authenticationFeatures == null)
				{
					authenticationFeatures = features;
					continue;
				}
				authenticationFeatures.EnableTokenRenewAction = authenticationFeatures.EnableTokenRenewAction || features.EnableTokenRenewAction;
				authenticationFeatures.EnableCredentialsAuthenticateAction = authenticationFeatures.EnableCredentialsAuthenticateAction || features.EnableCredentialsAuthenticateAction;
				authenticationFeatures.UseBearer = authenticationFeatures.UseBearer || features.UseBearer;
				authenticationFeatures.AuthenticationSchemes.AddRange(features.AuthenticationSchemes);
				MergeMappingTypes(authenticationFeatures, features.AuthenticationTypes);
			}

			if (authenticationFeatures != null)
			{
				services.AddSingleton(authenticationFeatures);
			}
			services.AddTransient<IClaimValueConvertService<DateTime, string>, DateTimeClaimValueConvertService>();

			if (!services.Any(sd => sd.ServiceType == typeof(IAuthenticationChangeInfoService)))
			{
				services.AddTransient<IAuthenticationChangeInfoService, EmptyAuthenticationChangeInfoService>();
			}

			return authenticationFeatures;
		}

		private static void MergeMappingTypes(IAuthenticationFeatures authenticationFeatures, List<Type> mappinTypes)
		{
			var authFeatures = authenticationFeatures as AuthenticationFeatures;
			var types = mappinTypes.Except(authFeatures.AuthenticationTypes).ToList();
			authFeatures.AddMappingTypes(types);
		}
	}

	public class AuthSettings
	{
		public string InitializerType { get; set; }

	}
}
