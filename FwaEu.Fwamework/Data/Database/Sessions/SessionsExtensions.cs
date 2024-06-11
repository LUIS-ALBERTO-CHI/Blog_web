using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public static class SessionsExtensions
	{
		public static void AddStatelessSessionResolver<TSession, TFactory>(this IServiceCollection services)
			where TSession : class, IStatelessSessionAdapter
			where TFactory : class, ISessionAdapterFactory
		{
			services.AddScoped<
				ISessionResolver<TSession>,
				StatelessSessionResolver<TSession, TFactory>>();
		}

		public static void AddStatefulSessionResolver<TSession, TFactory>(this IServiceCollection services)
			where TSession : class, IStatefulSessionAdapter
			where TFactory : class, ISessionAdapterFactory
		{
			services.AddScoped<
				ISessionResolver<TSession>,
				StatefulSessionResolver<TSession, TFactory>>();
		}

		public static IServiceCollection AddFwameworkSessionsServices(this IServiceCollection services)
		{
			services.AddStatelessSessionResolver<IStatelessSessionAdapter, ISessionAdapterFactory>();
			services.AddStatefulSessionResolver<IStatefulSessionAdapter, ISessionAdapterFactory>();

			services.AddScoped(typeof(IRepositorySessionFactory<>), typeof(DefaultRepositorySessionFactory<>));
			services.AddScoped<MainSessionContext>();

			return services;
		}
	}
}
