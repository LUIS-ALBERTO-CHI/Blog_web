using FwaEu.Fwamework.WebApi.Converters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public static class WebApiExtensions
	{
		public static IMvcBuilder AddFwameworkMvc(this IServiceCollection services, ApplicationInitializationContext context)
		{
			services.Configure<FormOptions>(config =>
			{
				// NOTE: Arbitrary value to allow the default length of Chrome separators -__-
				config.MultipartHeadersLengthLimit = MultipartReader.DefaultHeadersLengthLimit * 10;
			});

			var builder = services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.Formatting = context.Environment.IsDevelopment()
						? Newtonsoft.Json.Formatting.Indented
						: Newtonsoft.Json.Formatting.None;

					//NOTE: The DateTimeZoneHandling allows to always have only one DateTimeKind when sending a DateTime on WebAPI
					//By defaut we want to serve the dates as UTC
					options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.Converters.Add(new StringEnumConverter());

					//NOTE: The DateTimeForcedKindConverter allows to always have only one DateTimeKind when receiving a DateTime from WebAPI
					//By defaut we want to save the dates as Local to the database
					options.SerializerSettings.Converters.Add(new DateTimeForcedKindConverter(DateTimeKind.Local));
				});

			services.AddSingleton<IMVCJsonSerializer, NewtonsoftMVCJsonSerializer>();

			return builder;
		}

		public static IServiceCollection AddFwameworkApplicationAllower(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var serviceConfiguration = context.Configuration.GetSection("Fwamework:AllowedApplications:Service"); 
			services.Configure<AllowedApplicationsOptions>(serviceConfiguration);
			services.AddTransient<IApplicationAllowedCheckService, ApplicationAllowedCheckService>();
			
			return services;
		}

		public static IServiceCollection AddFwameworkWebApiAllower(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var serviceConfiguration = context.Configuration.GetSection("Fwamework:AllowedApplications:WebApi");
			services.Configure<AllowedWebApiOptions>(serviceConfiguration);

			return services;
		}
	}
}
