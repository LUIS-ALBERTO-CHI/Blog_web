using Microsoft.Extensions.DependencyInjection;
using System;

namespace FwaEu.Modules.GenericAdmin
{
	public class ModelAttributeGenericAdminModelConfigurationFactory<TConfiguration> : IGenericAdminConfigurationFactory
		where TConfiguration : IGenericAdminModelConfiguration
	{
		private string _key;
		public string Key => _key;

		public ModelAttributeGenericAdminModelConfigurationFactory(string key)
		{
			_key = key;
		}

		public IGenericAdminModelConfiguration Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<TConfiguration>();
		}
	}
}
