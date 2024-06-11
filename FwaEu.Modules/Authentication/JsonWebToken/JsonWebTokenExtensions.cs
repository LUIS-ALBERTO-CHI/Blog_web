using FwaEu.Fwamework;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken
{
	public static class JsonWebTokenExtensions
	{
		public static void AddJsonWebTokenServices(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:Authentication:JsonWebToken");
			services.Configure<JsonWebTokenOptions>(section);

			services.AddTransient<IJsonWebTokenWriter, JsonWebTokenWriter>();
			services.AddTransient<ITokenRenewerService, JsonWebTokenRenewerService>();

			services.AddTransient<IIdentityResolver, ClaimIdentityResolver>();
		}
	}
}
