using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FwaEu.Fwamework.StaticFiles
{
	public static class StaticFilesExtensions
	{
		public static IServiceCollection AddFwameworkStaticFilesServices(this IServiceCollection services)
		{
			services.AddSingleton<IContentTypeProvider, FwameworkFileExtensionContentTypeProvider>();
			return services;
		}
	}
}
