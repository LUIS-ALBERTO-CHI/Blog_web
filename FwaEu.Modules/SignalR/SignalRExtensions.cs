using FwaEu.Fwamework;
using FwaEu.Modules.Authentication.JwtBearerEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FwaEu.Modules.SignalR
{
	public static class SignalRExtensions
	{
		public static void AddFwameworkModuleSignalR(this IServiceCollection services, ApplicationInitializationContext context)
		{
			services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
			{
				var serializerSettings = ApplicationServices.ServiceProvider.GetService<IOptions<MvcNewtonsoftJsonOptions>>()?.Value?.SerializerSettings;
				if (serializerSettings != null)
				{
					options.PayloadSerializerSettings = serializerSettings;
				}
			});
			services.AddSingleton<IUserIdProvider, UserIdProvider>();
			services.AddTransient<IJwtBearerEventsHandler, SignalRJwtBearerEventsHandler>();
		}
	}
}
