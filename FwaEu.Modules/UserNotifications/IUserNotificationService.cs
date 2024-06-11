using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	public interface IUserNotificationService
	{
		Task SendNotificationAsync<TModel>(string notificationType,
			TModel model, IEnumerable<string> userIdentities, bool isSticky = false);
	}

	public static class UserNotificationServiceExtensions
	{
		public static Task SendNotificationAsync<TModel>(
			this IUserNotificationService userNotificationService,
			string notificationType, TModel model, string userIdentity, bool isSticky = false)
		{
			return userNotificationService.SendNotificationAsync(
				notificationType, model, new[] { userIdentity }, isSticky);
		}
	}
}
