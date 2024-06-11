using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.MasterDataUserNotifications;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterDataImporter
{
	public class EntityMasterDataImporterEventListenerFactory : IModelImporterEventListenerFactory<IEntityImporterEventListener>
	{
		public IEntityImporterEventListener Create(ServiceStore serviceStore, IServiceProvider serviceProvider)
		{
			return new ImportListener(serviceProvider);
		}
	}
	

	public class ImportListener : EmptyModelImporterEventListener, IEntityImporterEventListener
	{
		
		public IDisposable _disabler { get; }
		List<Type> savedModels = new List<Type>();
		public IServiceProvider _serviceProvider { get; }
		public ImportListener( IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_disabler = _serviceProvider.GetService<EntityChangeDectection.Disabler>().Disable();
		}

		public override Task OnModelSavedAsync(object model)
		{
			var modelType = model.GetType();
			if(!savedModels.Contains(modelType)) {
				savedModels.Add(modelType);
			}
			return Task.CompletedTask;
		}

		public override async Task OnImportFinished()
		{
			if (savedModels.Any())
			{
				var relatedMasterDataServices = _serviceProvider.GetServices<IMasterDataRelatedEntity>()
				.Where(x => savedModels.Contains(x.RelatedEntityType))
				.ToList();
				if (relatedMasterDataServices.Any())
				{
					var hubService = _serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
					var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
					var now = _serviceProvider.GetService<ICurrentDateTime>().Now;
					var clients = hubService.Clients.All;
					await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, masterDataKeys));
				}
			}
		}
		public override ValueTask DisposeAsync()
		{
			_disabler.Dispose();
			return ValueTask.CompletedTask;
		}

	}
}
