using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.HtmlRenderer;
using FwaEu.Modules.HtmlRenderer.Razor;
using FwaEu.Modules.Mail.HtmlMailSender;
using Microsoft.Extensions.DependencyInjection;
using static FwaEu.Modules.PasswordRecovery.DefaultUserPasswordRecoveryService;

namespace FwaEu.Modules.PasswordRecovery
{
	public static class UserPasswordRecoveryExtensions
	{
		public static IServiceCollection AddFwameworkModuleUserPasswordRecovery(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserPasswordRecoveryEntityRepository>();
			services.AddTransient<IUserPasswordRecoveryService, DefaultUserPasswordRecoveryService>();

			services.AddTransient<IRazorTemplateResolver<UserPasswordRecoveryMailModel>>(
			   sp => new EmbeddedResourceRazorTemplateResolver<UserPasswordRecoveryMailModel>());
			services.AddTransient<IHtmlRenderer<UserPasswordRecoveryMailModel>, RazorHtmlRenderer<UserPasswordRecoveryMailModel>>();
			services.AddTransient<IHtmlMailSender<UserPasswordRecoveryMailModel>, DefaultHtmlMailSender<UserPasswordRecoveryMailModel>>();
			return services;
		}
	}
}
