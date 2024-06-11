using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public interface IAuthenticationInitializer
	{
		IAuthenticationFeatures AddAuthentication(IServiceCollection services, ApplicationInitializationContext context);
	}
}
