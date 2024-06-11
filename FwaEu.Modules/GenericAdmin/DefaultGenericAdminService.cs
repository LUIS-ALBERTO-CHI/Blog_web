using System;

namespace FwaEu.Modules.GenericAdmin
{
	public class DefaultGenericAdminService : IGenericAdminService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly GenericAdminConfigurationFactoryCache _configurationFactories;

		public DefaultGenericAdminService(
			IServiceProvider serviceProvider,
			GenericAdminConfigurationFactoryCache configurationFactories)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_configurationFactories = configurationFactories ?? throw new ArgumentNullException(nameof(configurationFactories));
		}

		public IGenericAdminModelConfiguration GetConfiguration(string key)
		{
			return _configurationFactories.GetFactory(key)?.Create(_serviceProvider);
		}
	}
}
