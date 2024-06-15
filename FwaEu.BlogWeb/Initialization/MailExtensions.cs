using FwaEu.Fwamework;
using FwaEu.Fwamework.Mail;
using FwaEu.Fwamework.Setup;
using FwaEu.Modules.Mail.EmptyMailSender;
using FwaEu.Modules.Mail.SmtpMailSender;
using FwaEu.Modules.Mail.SmtpMailSender.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Initialization
{
	public static class MailExtensions
	{
		private static (Modules.Mail.SmtpMailSender.SmtpOptions Settings, IConfigurationSection Section)
			GetSmtpOptions(IConfiguration configuration)
		{
			var section = configuration.GetSection("Fwamework:Mail:Smtp");
			var settings = section.Exists() ? section.Get<Modules.Mail.SmtpMailSender.SmtpOptions>() 
				: default(Modules.Mail.SmtpMailSender.SmtpOptions);

			return (settings, section);
		}

		public static IServiceCollection AddApplicationMailServices(
			this IServiceCollection services, ApplicationInitializationContext context)
		{
			var SmtpOptions = GetSmtpOptions(context.Configuration);

			if (String.IsNullOrEmpty(SmtpOptions.Settings?.Host))
			{
				services.AddSingleton<MailSender, EmptyMailSender>();
			}
			else
			{
				services.Configure<Modules.HtmlRenderer.HtmlRendererOptions>(context.Configuration.GetSection(
					Modules.HtmlRenderer.HtmlRendererOptions.OptionPath));

				services.Configure<Modules.Mail.SmtpMailSender.SmtpOptions>(SmtpOptions.Section);
				services.AddSingleton<MailSender, SmtpMailSender>();
				services.AddTransient<ISetupTask, SendMailTask>();
			}

			services.AddTransient<ISetupTask, SmtpOptionsTask>();

			return services;
		}
	}
}
