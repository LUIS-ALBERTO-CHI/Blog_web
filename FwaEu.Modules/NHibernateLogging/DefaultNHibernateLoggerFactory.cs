using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public class DefaultNHibernateLoggerFactory : INHibernateLoggerFactory
	{
		private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;
		private readonly INHibernateLoggingInfo _nHibernateLoggingInfo;

		public DefaultNHibernateLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory,
			INHibernateLoggingInfo nHibernateLoggingInfo)
		{
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			_nHibernateLoggingInfo = nHibernateLoggingInfo ?? throw new ArgumentNullException(nameof(nHibernateLoggingInfo));
		}

		public INHibernateLogger LoggerFor(string keyName)
		{
			return new DefaultNHibernateLogger(_loggerFactory.CreateLogger(keyName),
				_nHibernateLoggingInfo.Enabled 
					&& _nHibernateLoggingInfo.LoggableNamespaces != null
						? _nHibernateLoggingInfo.LoggableNamespaces.FirstOrDefault(s => s == keyName) != null
						: false); //NOTE: Filter everything if nothing has been configured
		}

		public INHibernateLogger LoggerFor(Type type)
		{
			return LoggerFor(type.FullName);
		}
	}
}
