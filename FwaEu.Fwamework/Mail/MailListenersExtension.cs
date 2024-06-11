using FwaEu.Fwamework.Mail.EventListener;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Mail
{
	public static class MailListenersExtension
	{
		public static IServiceCollection AddMailListenersServices(this IServiceCollection services)
		{
			services.AddTransient<IMailListener, DefaultMailListener>();

			return services;
		}
	}
}
