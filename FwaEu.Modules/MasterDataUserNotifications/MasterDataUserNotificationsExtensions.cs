using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Monitoring;
using FwaEu.Modules.UserNotifications;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterDataUserNotifications
{
	public static class MasterDataUserNotificationsExtensions
	{
		public static IServiceCollection AddFwameworkModuleMasterDataUserNotifications(this IServiceCollection services)
		{
			services.AddTransient<IEventListenersInitializer, EntityChangeDectectionInitializer>();
			services.AddTransient<IFlushEntityEventListener, EntityChangeDectection>();
			services.AddScoped<EntityChangeDectection.Disabler>();

			return services;
		}
	}
}
