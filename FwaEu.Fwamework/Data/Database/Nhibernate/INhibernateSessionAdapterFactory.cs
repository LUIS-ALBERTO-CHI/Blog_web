using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface INhibernateSessionAdapterFactory : ISessionAdapterFactory
	{
		new INhibernateStatefulSessionAdapter CreateStatefulSession(CreateSessionOptions options = null);
		new INhibernateStatelessSessionAdapter CreateStatelessSession(CreateSessionOptions options = null);
	}
}
