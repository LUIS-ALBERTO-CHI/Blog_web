using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public class NHibernateLoggingOptions
	{
		public bool Enabled { get; set; }
		public IEnumerable<string> LoggableNamespaces { get; set; }
	}
}
