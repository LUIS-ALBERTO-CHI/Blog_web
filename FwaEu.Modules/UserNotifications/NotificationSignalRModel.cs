using System;

namespace FwaEu.Modules.UserNotifications
{
	public class NotificationSignalRModel
	{
		public NotificationSignalRModel(Guid id, DateTime sentOn, object model)
		{
			this.Id = id;
			this.SentOn = sentOn;
			this.Model = model;
		}

		public Guid Id { get; }
		public DateTime SentOn { get; }
		public object Model { get; }
	}
}
