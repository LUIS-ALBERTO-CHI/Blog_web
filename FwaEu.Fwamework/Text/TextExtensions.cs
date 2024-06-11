using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Text
{
	public static class TextExtensions
	{
		public static IServiceCollection AddFwameworkTextServices(this IServiceCollection services)
		{
			services.AddSingleton<IPluralizationService, PluralizeNetCorePluralizationService>();
			return services;
		}
	}
}
