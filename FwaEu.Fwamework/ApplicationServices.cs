using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework
{
	public static class ApplicationServices
	{
		public static IServiceProvider ServiceProvider { get; private set; }

		public static void Initialize(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}
	}
}
