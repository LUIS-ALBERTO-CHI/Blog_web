using FwaEu.Fwamework.Setup.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public static class ModelValidationExtensions
	{
		public static IServiceCollection AddFwameworkModelValidation(this IServiceCollection services)
		{
			services.AddTransient<IModelValidationService, DefaultModelValidationService>();

			return services;
		}
	}
}
