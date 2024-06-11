using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.DependencyInjection
{
	public interface IScopedServiceProviderAccessor //NOTE: Should be used only to use old components which do not supports dependency injection
	{
		IServiceProvider GetScopeServiceProvider();
    }

    public interface IScopedServiceProvider
    {
        IServiceProvider GetScopeServiceProvider();
    }
}
