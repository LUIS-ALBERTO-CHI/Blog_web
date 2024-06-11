using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors
{
	public interface IInterceptorFactory
	{
		NHibernate.IInterceptor CreateInterceptor(
			CreateSessionOptions options,
			IServiceProvider serviceProvider);
	}
}
