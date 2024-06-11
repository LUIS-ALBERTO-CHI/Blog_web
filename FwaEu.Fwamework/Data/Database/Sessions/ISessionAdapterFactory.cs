using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public interface ISessionAdapterFactory
	{
		IStatefulSessionAdapter CreateStatefulSession(CreateSessionOptions options = null);
		IStatelessSessionAdapter CreateStatelessSession(CreateSessionOptions options = null);
	}

	public class CreateSessionOptions
	{
		public string ConnectionStringName { get; set; }
		public DbConnection Connection { get; set; }
	}
}
