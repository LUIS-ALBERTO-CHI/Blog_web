using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	public interface IUserNotificationStore
	{
		/// <returns>An unique identifier associated to the stored user notification.</returns>
		Task<Guid> SaveAsync(UserNotificationStoreModel model,
			string[] userIdentities);

		Task<IEnumerable<StoredNotificationModel>> GetNotificationsAsync(int userId);

		Task MarkAsSeenAsync(int userId, DateTime date);

		Task DeleteAsync(int userId, Guid notificationId);
	}

	public class StoredNotificationModel : NotificationSignalRModel
	{
		public StoredNotificationModel(Guid id,string notificationType, DateTime sentOn, object model, DateTime? seenOn, bool isSticky)
			: base(id, sentOn, model)
		{
			this.NotificationType = notificationType;
			this.SeenOn = seenOn;
			this.IsSticky = isSticky;
		}
		public string NotificationType { get; }
		public DateTime? SeenOn { get; }
		public bool IsSticky { get; set; }
	}

	public class UserNotificationStoreModel
	{
		public UserNotificationStoreModel(string notificationType, object argument, DateTime sentOn, bool isSticky)
		{
			this.NotificationType = notificationType
				?? throw new ArgumentNullException(nameof(notificationType));

			this.Argument = argument;
			this.SentOn = sentOn;
			IsSticky = isSticky;
		}

		public string NotificationType { get; }
		public object Argument { get; }
		public DateTime SentOn { get; }
		public bool IsSticky { get; set; }
	}
}