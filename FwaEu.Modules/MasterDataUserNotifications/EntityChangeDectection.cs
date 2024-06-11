using FwaEu.Fwamework;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.MasterData.WebApi;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Event;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Aspose.Cells;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;

namespace FwaEu.Modules.MasterDataUserNotifications
{
	public class EntityChangeDectection : IFlushEntityEventListener
	{
		private readonly IScopedServiceProvider _scopedServiceProvider;

		public IHubContext<UserNotificationHub, IUserNotificationClient> HubContext { get; }
		public EntityChangeDectection(IScopedServiceProvider scopedServiceProvider)
		{
			this._scopedServiceProvider = scopedServiceProvider
				?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}
		public void OnFlushEntity(FlushEntityEvent @event)
		{
		}

		public async Task OnFlushEntityAsync(FlushEntityEvent @event, CancellationToken cancellationToken)
		{
			if (@event.DirtyProperties == null || @event.DirtyProperties.Length == 0) {
				return;
			}
			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var entityType = @event.Entity.GetType();

			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>()
			.Where(x => x.RelatedEntityType == entityType)
			.ToList();
			if (relatedMasterDataServices.Any())
			{
				var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
				var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
				var now = serviceProvider.GetService<ICurrentDateTime>().Now;
				var clients = hubService.Clients.All;
				await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, masterDataKeys));
			}
		}
		private class DisablerState : IDisposable
		{
			public bool IsDisabled { get; private set; }

			public DisablerState()
			{
				this.IsDisabled = true;
			}
			public void Dispose()
			{
				this.IsDisabled = false;
			}
		}

		public class Disabler
		{
			private DisablerState disablerState;

			public IDisposable Disable()
			{
				this.disablerState = new DisablerState();
				return this.disablerState;
			}

			public bool IsDisabled()
			{
				return this.disablerState != null && this.disablerState.IsDisabled;
			}
		}

	}
}
