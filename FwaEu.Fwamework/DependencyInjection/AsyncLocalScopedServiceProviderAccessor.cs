using System;
using System.Threading;

namespace FwaEu.Fwamework.DependencyInjection
{

    //Note: taken from https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/HttpContextAccessor.cs
    public class AsyncLocalScopedServiceProviderAccessor : IScopedServiceProviderAccessor
    {
        public IServiceProvider GetScopeServiceProvider()
        {
            return ScopedServiceProvider;
        }

        private static readonly AsyncLocal<IServiceProvider> _scopedServiceProviderCurrent = new AsyncLocal<IServiceProvider>();

        private IServiceProvider ScopedServiceProvider
        {
            get => _scopedServiceProviderCurrent.Value;
            set => _scopedServiceProviderCurrent.Value = value;
        }

        public IDisposable BeginScope(IServiceProvider scopedServiceProvider)
        {
            return new ScopedServiceProviderDisposer(this, scopedServiceProvider);
        }

        private class ScopedServiceProviderDisposer : IDisposable
        {
            private readonly AsyncLocalScopedServiceProviderAccessor _asyncLocalScopedServiceProviderAccessor;

            public ScopedServiceProviderDisposer(AsyncLocalScopedServiceProviderAccessor asyncLocalScopedServiceProviderAccessor, IServiceProvider scopedServiceProvider)
            {
                this._asyncLocalScopedServiceProviderAccessor = asyncLocalScopedServiceProviderAccessor;
                this._asyncLocalScopedServiceProviderAccessor.ScopedServiceProvider = scopedServiceProvider;
            }

            public void Dispose()
            {
                this._asyncLocalScopedServiceProviderAccessor.ScopedServiceProvider = null;
            }
        }
    }
}
