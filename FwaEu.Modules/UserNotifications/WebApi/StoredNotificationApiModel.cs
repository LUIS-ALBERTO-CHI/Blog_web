using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications.WebApi
{
	// NOTE: The signature of the api model must be exactly the same as signalR model, to make handling on client-side easier
	public class StoredNotificationApiModel : NotificationSignalRModel
	{
		public StoredNotificationApiModel(Guid id, string notificationType, DateTime sentOn, object model, DateTime? seenOn, bool isSticky)
			: base(id, sentOn, model)
		{
			this.NotificationType = notificationType;
			this.SeenOn = seenOn;
			this.IsSticky = isSticky;
		}
		public string NotificationType { get; }
		public bool IsSticky { get; }

		public DateTime? SeenOn { get; }
	}
}
