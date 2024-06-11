using FwaEu.Fwamework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.NHibernateLogging
{
	public static  class NHibernateLoggingExtensions
	{
		public static IServiceCollection AddFwameworkModuleNHibernateLogging(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Application:NHibernateLogging");
			services.Configure<NHibernateLoggingOptions>(section);
			services.AddSingleton<INHibernateLoggingInfo, DefaultNHibernateLoggingInfo>();

			return services;
		}

		public static IApplicationBuilder ConfigureNHibernateLogging(this IApplicationBuilder builder,
			Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, INHibernateLoggingInfo nHibernateLoggingInfo)
		{
			NHibernateLogger.SetLoggersFactory(new DefaultNHibernateLoggerFactory(loggerFactory, nHibernateLoggingInfo));

			return builder;
		}
	}
}
