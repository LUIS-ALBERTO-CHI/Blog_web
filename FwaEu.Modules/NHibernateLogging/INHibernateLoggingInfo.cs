using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public interface INHibernateLoggingInfo
	{
		bool Enabled { get; }
		IEnumerable<string> LoggableNamespaces { get; }
	}
}
