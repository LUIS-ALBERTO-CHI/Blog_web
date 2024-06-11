using FwaEu.Fwamework;
using FwaEu.Modules.Users.Avatars;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AvatarsGravatar
{
	public static class AvatarsGravatarExtensions
	{
		public static IServiceCollection AddFwameworkModuleGravatar(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:Gravatar");
			services.Configure<GravatarOptions>(section);

			services.AddTransient<IAvatarService, GravatarAvatarService>();
			return services;
		}
	}
}
