using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.GenericAdmin.WebApi;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FwaEu.Modules.GenericAdmin
{
	public static class GenericAdminExtensions
	{
		public static IServiceCollection AddFwameworkModuleGenericAdmin(this IServiceCollection services)
		{
			services.AddSingleton<GenericAdminConfigurationFactoryCache>();
			services.AddTransient<IGenericAdminService, DefaultGenericAdminService>();
			services.AddTransient<IDataSourceModelFactory, ListDataSourceModelFactory>();
			services.AddTransient<IDataSourceModelFactory, EnumDataSourceModelFactory>();
			return services;
		}

		public static IServiceCollection AddGenericAdminConfiguration<TConfiguration>(this IServiceCollection services, string key = null)
			where TConfiguration : class, IGenericAdminModelConfiguration
		{
			if (key == null)
			{
				var typeEntity = typeof(IEntity);
				var configurationEntityType = typeof(TConfiguration).BaseType.GenericTypeArguments.FirstOrDefault(x => typeEntity.IsAssignableFrom(x));
				if (configurationEntityType == null)
					throw new NotImplementedException($"Cannot find a key for GenericAdmin configuration '{typeof(TConfiguration).Name}'");

				var keyProviderType = typeof(IEntityKeyResolver<>).MakeGenericType(configurationEntityType);
				services.AddTransient<IGenericAdminConfigurationFactory>(sp => {
					return new ModelAttributeGenericAdminModelConfigurationFactory<TConfiguration>
					(
						((IEntityKeyResolver)sp.GetService(keyProviderType)).ResolveKey()
					);
				});
			}
			else
			{
				services.AddTransient<IGenericAdminConfigurationFactory>(sp => new ModelAttributeGenericAdminModelConfigurationFactory<TConfiguration>(key));
			}

			services.AddTransient<TConfiguration>();

			return services;
		}
	}
}
