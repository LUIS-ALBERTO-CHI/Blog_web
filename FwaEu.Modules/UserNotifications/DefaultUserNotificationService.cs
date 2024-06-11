using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Temporal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	public class DefaultUserNotificationService : IUserNotificationService
	{
		public DefaultUserNotificationService(
			IHubContext<UserNotificationHub, IUserNotificationClient> hubContext,
			IUserNotificationStore userNotificationStore,
			ICurrentDateTime currentDateTime,
			IServiceProvider serviceProvider)
		{
			this.HubContext = hubContext
				?? throw new ArgumentNullException(nameof(hubContext));

			this.UserNotificationStore = userNotificationStore
				?? throw new ArgumentNullException(nameof(userNotificationStore));

			this.CurrentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public IHubContext<UserNotificationHub, IUserNotificationClient> HubContext { get; }
		protected IUserNotificationStore UserNotificationStore { get; }
		protected ICurrentDateTime CurrentDateTime { get; }
		protected IServiceProvider ServiceProvider { get; }

		public async Task SendNotificationAsync<TModel>(string notificationType,
			TModel model, IEnumerable<string> userIdentities, bool isSticky)
		{
			var userIdentitiesArray = userIdentities.ToArray();
			var now = this.CurrentDateTime.Now;

			var storeIdentifier = await this.UserNotificationStore.SaveAsync(
				new UserNotificationStoreModel(notificationType, model, now, isSticky),
				userIdentitiesArray);

			var clients = this.HubContext.Clients.Users(userIdentitiesArray);

			await clients.SendAsync(notificationType,
				new NotificationSignalRModel(storeIdentifier, now, model));
		}
	}
}
