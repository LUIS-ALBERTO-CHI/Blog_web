using FwaEu.Fwamework.Caching;
using FwaEu.Fwamework.Data.Database.Nhibernate.Cache;
using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public static class NhibernateExtensions
	{
		public static void AddSessionResolvers(IServiceCollection services)
		{
			services.AddStatelessSessionResolver<INhibernateStatelessSessionAdapter, INhibernateSessionAdapterFactory>();
			services.AddStatefulSessionResolver<INhibernateStatefulSessionAdapter, INhibernateSessionAdapterFactory>();
		}

		public static IServiceCollection AddFwameworkNhibernate(this IServiceCollection services)
		{
			services.AddSingleton<ISessionFactoryProvider, SessionFactoryProvider>();

			AddSessionResolvers(services);

			services.AddSingleton<INhibernateSessionAdapterFactory, SessionAdapterFactory>();
			services.AddSingleton<ISessionAdapterFactory>(sp => sp.GetService<INhibernateSessionAdapterFactory>());

			services.AddTransient<ICacheManager, NhibernateCacheManager>();
			services.AddTransient<IEventListenersInitializer, CreationOrUpdateTrackingEventListenersInitializer>();
			services.AddScoped<CreationOrUpdateTrackingEventListener.Disabler>();
			return services;
		}
	}
}
