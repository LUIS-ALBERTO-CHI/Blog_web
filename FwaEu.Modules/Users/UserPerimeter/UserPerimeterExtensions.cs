using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Modules.Users.UserPerimeter.Parts;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public static class UserPerimeterExtensions
	{
		public static IServiceCollection AddFwameworkModuleUserPerimeterServices(this IServiceCollection services)
		{
			services.AddSingleton<UserPerimeterProviderFactoryCache>();
			services.AddTransient<IUserPerimeterService, DefaultUserPerimeterService>();

			services.AddTransient<IPartHandler, UserPerimetersPartHandler>();

			services.AddScoped<ICurrentUserPerimeterService, DefaultCurrentUserPerimeterService>();

			return services;
		}

		public static IServiceCollection AddUserPerimeterProvider<TProvider>(
			this IServiceCollection services, string key)
			where TProvider : class, IUserPerimeterProvider
		{
			services.AddTransient<IUserPerimeterProviderFactory>(
				sp => new InjectedUserPerimeterProviderFactory<TProvider>(key));
			
			services.AddTransient<TProvider>();

			return services;
		}
	}
}
