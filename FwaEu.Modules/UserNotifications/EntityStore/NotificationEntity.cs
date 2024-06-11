using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications.EntityStore
{
	public class NotificationEntity : IEntity, ICreatedByTracked
	{
		public Guid Id { get; set; }
		public string NotificationType { get; set; }
		public string Argument { get; set; }
		public bool IsSticky { get; set; }
		public DateTime SentOn { get; set; }
		public UserEntity CreatedBy { get; set; }

		public bool IsNew()
		{
			//NOTE: No need for implementation currently (inherited from ICreationTracked -> IEntity)
			throw new NotSupportedException();
		}
	}

	public class NotificationEntityRepository : DefaultRepository<NotificationEntity, Guid>
	{
		public IQueryable<NotificationEntity> QueryOrphans()
		{
			var context = this.ServiceProvider.GetRequiredService<MainSessionContext>();

			var recipientsQuery = context.RepositorySession
				.Create<NotificationRecipientEntityRepository>()
				.QueryNoPerimeter();

			return this.Query()
				.Where(n => !recipientsQuery
					.Any(nr => nr.PrimaryKey.Notification.Id == n.Id));
		}
	}

	public class NotificationEntityClassMap : ClassMap<NotificationEntity>
	{
		public NotificationEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Assigned();
			
			Map(entity => entity.NotificationType).Not.Nullable();
			Map(entity => entity.Argument);
			Map(entity => entity.IsSticky).Default("0");
			Map(entity => entity.SentOn);

			References(entity => entity.CreatedBy).Nullable();
		}
	}
}
