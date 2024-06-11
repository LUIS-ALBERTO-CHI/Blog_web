using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework
{
	public class ApplicationInitializationContext : IDisposable
	{
		public ApplicationInitializationContext(
			IConfiguration configuration,
			IWebHostEnvironment environment)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }
		public ServiceStore ServiceStore { get; } = new ServiceStore(ServiceStoreInnerServicesLifetime.Singleton);

		public void Dispose()
		{
			this.ServiceStore.Dispose();
		}
	}
}
