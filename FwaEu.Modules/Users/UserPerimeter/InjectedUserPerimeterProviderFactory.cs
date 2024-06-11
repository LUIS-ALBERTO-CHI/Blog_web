using Microsoft.Extensions.DependencyInjection;
using System;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public class InjectedUserPerimeterProviderFactory<TProvider> : IUserPerimeterProviderFactory
		where TProvider : class, IUserPerimeterProvider
	{
		public InjectedUserPerimeterProviderFactory(string key)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
		}

		public string Key { get; }

		public IUserPerimeterProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<TProvider>();
		}
	}
}
