using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Fwamework.DependencyInjection
{

    public class ScopedServiceProvider : IScopedServiceProvider
    {
        private readonly IEnumerable<IScopedServiceProviderAccessor> _scopedServiceProviderAccessors;

        public ScopedServiceProvider(IEnumerable<IScopedServiceProviderAccessor> scopedServiceProviderAccessors)
        {
            this._scopedServiceProviderAccessors = scopedServiceProviderAccessors;
        }

        public IServiceProvider GetScopeServiceProvider()
        {
            return this._scopedServiceProviderAccessors.Select(sc => sc.GetScopeServiceProvider()).FirstOrDefault(sc => sc != null);
        }
    }
}
