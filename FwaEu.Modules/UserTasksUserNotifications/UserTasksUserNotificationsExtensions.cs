using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasksUserNotifications
{
	public static class UserTasksUserNotificationsExtensions
	{
		public static void AddFwameworkModuleUserTasksUserNotifications(this IServiceCollection services)
		{
			services.AddTransient<UserTaskUserNotificationService>();
		}
	}
}
