using FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public class SessionAdapterFactory : INhibernateSessionAdapterFactory
	{
		public SessionAdapterFactory(
			ISessionFactoryProvider sessionFactoryProvider,
			IServiceProvider serviceProvider,
			IInterceptorFactory interceptorFactory)
		{
			this._sessionFactoryProvider = sessionFactoryProvider
				?? throw new ArgumentNullException(nameof(sessionFactoryProvider));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._interceptorFactory = interceptorFactory;
		}

		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private readonly IServiceProvider _serviceProvider;
		private readonly IInterceptorFactory _interceptorFactory;

		public INhibernateStatefulSessionAdapter CreateStatefulSession(CreateSessionOptions options = null)
		{
			var builder = this._sessionFactoryProvider.GetFactory(options).WithOptions();

			if (options?.Connection != null)
			{
				builder.Connection(options.Connection);
			}

			if (this._interceptorFactory != null)
			{
				var interceptor = this._interceptorFactory.CreateInterceptor(options, this._serviceProvider);
				if (interceptor != null)
				{
					builder.Interceptor(interceptor);
				}
			}

			return new StatefulSessionAdapter(builder.OpenSession());
		}

		public INhibernateStatelessSessionAdapter CreateStatelessSession(CreateSessionOptions options = null)
		{
			var builder = this._sessionFactoryProvider.GetFactory(options).WithStatelessOptions();

			if (options?.Connection != null)
			{
				builder.Connection(options.Connection);
			}

			return new StatelessSessionAdapter(builder.OpenStatelessSession());
		}

		IStatefulSessionAdapter ISessionAdapterFactory.CreateStatefulSession(CreateSessionOptions options)
		{
			return this.CreateStatefulSession(options);
		}

		IStatelessSessionAdapter ISessionAdapterFactory.CreateStatelessSession(CreateSessionOptions options)
		{
			return this.CreateStatelessSession(options);
		}
	}
}
