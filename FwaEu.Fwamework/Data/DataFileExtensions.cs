using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Data
{
	public static class DataFileExtensions
	{
		public static IServiceCollection AddFwameworkDataFileServices(this IServiceCollection services)
		{
			services.AddTransient<IFileNameCleaner, DefaultFileNameCleaner>();

			return services;
		}
	}
}
