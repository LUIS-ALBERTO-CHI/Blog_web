using FwaEu.Fwamework.Globalization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Initialization
{
	public static class GlobalizationExtensions
	{
		/// <summary>
		/// First culture is the default culture of the application.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<CultureInfo> GetCultures()
		{
			yield return new CultureInfo("en-US");
			yield return new CultureInfo("fr-FR");
		}

		public static IServiceCollection AddApplicationGlobalization(this IServiceCollection services)
		{
			var cultures = GetCultures().ToArray();
			services.AddSingleton<ICulturesService>(
				new DefaultCulturesService(cultures.First(), cultures));

			return services;
		}
	}
}
