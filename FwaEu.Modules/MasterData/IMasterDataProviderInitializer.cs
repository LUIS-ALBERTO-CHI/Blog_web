using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IMasterDataProviderInitializer
	{
		Func<IServiceProvider, string> ResolveKey { get; }
		IServiceCollection ServiceCollection { get; }
	}


	public class MasterDataProviderInitializer : IMasterDataProviderInitializer
	{
		public Func<IServiceProvider, string> ResolveKey { get; }
		public IServiceCollection ServiceCollection { get; }

	public MasterDataProviderInitializer(Func<IServiceProvider, string> resolveKey, IServiceCollection serviceCollection)
		{
			ResolveKey = resolveKey;
			ServiceCollection = serviceCollection;
		}
	}
}
