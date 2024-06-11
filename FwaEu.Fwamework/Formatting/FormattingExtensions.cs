using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Formatting
{
	public static class FormattingExtensions
	{
		public static IServiceCollection AddFwameworkFormatting(this IServiceCollection services)
		{
			services.AddSingleton<IToStringService, DefaultToStringService>();
			return services;
		}
	}
}
