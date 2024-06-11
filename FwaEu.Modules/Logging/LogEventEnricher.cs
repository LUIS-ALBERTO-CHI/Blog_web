using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Logging
{
	public class LogEventEnricher : ILogEventEnricher
	{
		private readonly IServiceProvider _serviceProvider;

		public LogEventEnricher(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			foreach (var enricherProperty in this._serviceProvider
				.GetServices<ILogEnricherProvider>()
				.SelectMany(provider => provider.GetProperties()))
			{
				logEvent.AddOrUpdateProperty(
					propertyFactory.CreateProperty(
						enricherProperty.Name,
						enricherProperty.Value,
						enricherProperty.Destructure));
			}
		}
	}
}
