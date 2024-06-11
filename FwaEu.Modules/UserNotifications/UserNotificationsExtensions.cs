using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.UserNotifications.EntityStore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	public static class UserNotificationsExtensions
	{
		public static void AddFwameworkModuleUserNotifications(
			this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<NotificationEntityRepository>();
			repositoryRegister.Add<NotificationRecipientEntityRepository>();

			services.AddTransient<IUserNotificationService, DefaultUserNotificationService>();
			services.AddTransient<IUserNotificationStore, EntityUserNotificationStore>();
		}
	}
}