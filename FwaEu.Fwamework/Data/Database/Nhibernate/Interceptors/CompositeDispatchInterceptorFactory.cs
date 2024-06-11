using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors
{
	public class CompositeDispatchInterceptorFactory : IInterceptorFactory
	{
		public IInterceptor CreateInterceptor(
			CreateSessionOptions options,
			IServiceProvider serviceProvider)
		{
			return new CompositeDispatchInterceptor(serviceProvider);
		}
	}
}
