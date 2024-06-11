using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications.EntityStore
{
	public class NotificationRecipientPrimaryKey
	{
		public NotificationEntity Notification { get; set; }
		public UserEntity User { get; set; }

		public override bool Equals(object obj)
		{
			return obj is NotificationRecipientPrimaryKey pk
				&& pk.Notification == this.Notification
				&& pk.User == pk.User;
		}

		public override int GetHashCode()
		{
			return $"{this.Notification.Id}_{this.User.Id}".GetHashCode();
		}
	}

	public class NotificationRecipientEntity
	{
		public NotificationRecipientEntity() // NOTE: For Nhibernate and UpdateAsync()
		{
		}

		public NotificationRecipientEntity(NotificationRecipientPrimaryKey primaryKey)
		{
			this.PrimaryKey = primaryKey
				?? throw new ArgumentNullException(nameof(primaryKey));
		}

		public NotificationRecipientPrimaryKey PrimaryKey { get; protected set; }
		public DateTime? SeenOn { get; set; }
	}

	public class NotificationRecipientEntityClassMap : ClassMap<NotificationRecipientEntity>
	{
		public NotificationRecipientEntityClassMap()
		{
			Not.LazyLoad();

			CompositeId(entity => entity.PrimaryKey)
				.KeyReference(pk => pk.Notification, "notification_id")
				.KeyReference(pk => pk.User, "user_id");

			Map(entity => entity.SeenOn);
		}
	}

	public class NotificationRecipientEntityRepository : DefaultRepository<NotificationRecipientEntity, NotificationRecipientPrimaryKey>
	{
		public IQueryable<NotificationRecipientEntity> QueryByUser(int userId)
		{
			return this.Query()
				.Where(nr => nr.PrimaryKey.User.Id == userId);
		}

		public IQueryable<NotificationRecipientEntity> QueryNotSeenByUser(int userId)
		{
			return this.QueryByUser(userId)
				.Where(nr => nr.SeenOn == null);
		}

		public IQueryable<NotificationRecipientEntity> QueryNotificationByUser(int userId, Guid notificationId)
		{
			return this.QueryByUser(userId)
				.Where(nr => nr.PrimaryKey.Notification.Id == notificationId);
		}
	}
}
