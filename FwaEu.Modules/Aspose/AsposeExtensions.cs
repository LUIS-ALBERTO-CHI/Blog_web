using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Aspose
{
	public static class AsposeExtensions
	{
		/// <param name="asposeLicenses">Ex: AsposeLicense.Cells</param>
		public static IServiceCollection AddFwameworkModuleAspose(this IServiceCollection services, params Type[] licenseTypes)
		{
			if (!licenseTypes.Any())
				throw new ArgumentException("You must provide at least one AsposeLicense to enable.", nameof(licenseTypes));

			foreach (var type in licenseTypes)
			{
				var license = new AsposeLicense(type);
				license.SetLicense();
			}

			return services;
		}
	}
}
