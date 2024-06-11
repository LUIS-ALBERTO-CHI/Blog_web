using FwaEu.Fwamework;
using FwaEu.Fwamework.Mail;
using FwaEu.Modules.Mail.HtmlMailSender;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.TemplateCore.Html.Mail
{
	public static class HtmlMailExtensions
	{

		public static IServiceCollection AddHtmlMailExtensions(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:MailContentPath");

			services.AddTransient<IHtmlMailParametersProvider, DefaultHtmlMailParametersProvider>();
			services.AddTransient<IHtmlMailContentPathProvider, DefaultHtmlMailContentPathProvider>();
			services.Configure<HtmlMailContentPathOptions>(section);
			services.AddSingleton<IHtmlMailContentPath, DefaultHtmlMailContentPath>();
			return services;
		}
	}
}
