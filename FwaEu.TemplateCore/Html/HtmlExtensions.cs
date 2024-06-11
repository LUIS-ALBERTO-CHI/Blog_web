using FwaEu.Fwamework;
using FwaEu.Modules.HtmlRenderer.Razor;
using FwaEu.TemplateCore.Html.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.TemplateCore.Html
{
	public static class HtmlMailsExtensions
	{
		public static IServiceCollection AddApplicationHtml(this IServiceCollection services, ApplicationInitializationContext context)
		{
			services.AddSingleton<IRazorEngine, DefaultRazorEngine>();
			services.AddHostedService(sp => sp.GetRequiredService<IRazorEngine>());
			services.AddSingleton<IHtmlRazorLayoutPathResolver>(sp => new DefaultHtmlRazorLayoutPathResolver("Html/Mail/_Layout"));
			return services.AddHtmlMailExtensions(context);
		}
	}
}
