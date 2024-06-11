using Microsoft.Extensions.Logging;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public class DefaultNHibernateLogger : INHibernateLogger
	{
		private static readonly IDictionary<NHibernateLogLevel, LogLevel> _logLevelFor = new Dictionary<NHibernateLogLevel, LogLevel>
		{
			[NHibernateLogLevel.Trace] = LogLevel.Trace,
			[NHibernateLogLevel.Debug] = LogLevel.Debug,
			[NHibernateLogLevel.Info] = LogLevel.Information,
			[NHibernateLogLevel.Warn] = LogLevel.Warning,
			[NHibernateLogLevel.Error] = LogLevel.Error,
			[NHibernateLogLevel.Fatal] = LogLevel.Critical,
			[NHibernateLogLevel.None] = LogLevel.None
		};
		private readonly ILogger _logger;
		private readonly bool _loggable;

		public DefaultNHibernateLogger(ILogger logger, bool loggable)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_loggable = loggable;
		}

		public bool IsEnabled(NHibernateLogLevel logLevel)
		{
			return _loggable && _logger.IsEnabled(_logLevelFor[logLevel]);
		}

		private string FormatLogValues(NHibernateLogValues state)
		{
			if (state.Args.Length > 0)
				return String.Format(state.Format, state.Args);
			return state.Format;
		}

		public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
		{
			if (_loggable)
			{
				//TODO: Implement logic to replace parameters in sql query https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/6185
				_logger.Log(_logLevelFor[logLevel], exception, "{1}", FormatLogValues(state));
			}
		}
	}
}
