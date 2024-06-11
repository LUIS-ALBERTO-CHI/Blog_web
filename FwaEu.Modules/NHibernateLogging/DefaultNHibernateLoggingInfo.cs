using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public class DefaultNHibernateLoggingInfo : INHibernateLoggingInfo
	{
		public DefaultNHibernateLoggingInfo(IOptions<NHibernateLoggingOptions> settings)
		{
			_ = settings ?? throw new ArgumentNullException(nameof(settings));

			this.LoggableNamespaces = settings.Value?.LoggableNamespaces;
			this.Enabled = settings.Value != null ? settings.Value.Enabled : false;
		}

		public bool Enabled { get; }
		public IEnumerable<string> LoggableNamespaces { get; }
	}
}
