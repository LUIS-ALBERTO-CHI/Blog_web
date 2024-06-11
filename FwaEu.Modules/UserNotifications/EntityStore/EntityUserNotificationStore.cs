using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications.EntityStore
{
	public class EntityUserNotificationStore : IUserNotificationStore
	{
		private readonly MainSessionContext _mainSessionContext;

		public EntityUserNotificationStore(MainSessionContext mainSessionContext)
		{
			this._mainSessionContext = mainSessionContext
				?? throw new ArgumentNullException(nameof(mainSessionContext));
		}

		private static string Serialize(object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}

		private static Dictionary<string, object> Deserialize(string storedArgument)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<
				Dictionary<string, object>>(storedArgument);
		}

		public async Task<IEnumerable<StoredNotificationModel>> GetNotificationsAsync(int userId)
		{
			var models = await this._mainSessionContext.RepositorySession
				.Create<NotificationRecipientEntityRepository>()
				.QueryByUser(userId)
				.Select(nr => new StoredNotificationModel(
					nr.PrimaryKey.Notification.Id, nr.PrimaryKey.Notification.NotificationType, nr.PrimaryKey.Notification.SentOn,
					Deserialize(nr.PrimaryKey.Notification.Argument), nr.SeenOn, nr.PrimaryKey.Notification.IsSticky))
				.ToListAsync();

			return models;
		}

		public async Task<Guid> SaveAsync(UserNotificationStoreModel model,
			string[] userIdentities)
		{
			if (userIdentities == null || userIdentities.Length == 0)
			{
				throw new ArgumentException(
					"You must provide at least one recipient (user identity).",
					nameof(userIdentities));
			}

			var repositorySession = this._mainSessionContext.RepositorySession;

			var recipients = await repositorySession.Create<IUserEntityRepository>()
				.Query().Where(u => userIdentities.Contains(u.Identity))
				.ToListAsync();

			if (recipients.Count != userIdentities.Length)
			{
				throw new NotSupportedException(
					"At least one user was not found with the specified user identities.");
			}

			var notificationRepository = repositorySession.Create<NotificationEntityRepository>();
			var notificationRecipientRepository = repositorySession.Create<NotificationRecipientEntityRepository>();

			var notification = new NotificationEntity()
			{
				Id = Guid.NewGuid(),
				NotificationType = model.NotificationType,
				Argument = model.Argument == null ? null : Serialize(model.Argument),
				SentOn = model.SentOn,
				IsSticky = model.IsSticky,
			};

			await notificationRepository.SaveOrUpdateAsync(notification);

			foreach (var recipient in recipients)
			{
				var entity = new NotificationRecipientEntity(
					new NotificationRecipientPrimaryKey()
					{
						Notification = notification,
						User = recipient,
					});

				await notificationRecipientRepository.SaveOrUpdateAsync(entity);
			}

			await repositorySession.Session.FlushAsync();

			return notification.Id;
		}

		public async Task MarkAsSeenAsync(int userId, DateTime date)
		{
			var repositorySession = this._mainSessionContext.RepositorySession;

			await repositorySession.Create<NotificationRecipientEntityRepository>()
				.QueryNotSeenByUser(userId)
				.UpdateAsync(nr => new NotificationRecipientEntity { SeenOn = date });

			await repositorySession.Session.FlushAsync();
		}

		public async Task DeleteAsync(int userId, Guid notificationId)
		{
			var repositorySession = this._mainSessionContext.RepositorySession;

			using (var transaction = repositorySession.Session.BeginTransaction())
			{
				await repositorySession.Create<NotificationRecipientEntityRepository>()
					.QueryNotificationByUser(userId, notificationId)
					.DeleteAsync();

				// NOTE: Delete all orphans... Less code, and will clean the database if trashed

				await repositorySession.Create<NotificationEntityRepository>()
					.QueryOrphans()
					.DeleteAsync();

				await repositorySession.Session.FlushAsync();
				await transaction.CommitAsync();
			}
		}
	}
}
