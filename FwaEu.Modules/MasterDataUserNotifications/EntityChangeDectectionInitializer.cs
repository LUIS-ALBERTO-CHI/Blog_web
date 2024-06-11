using System.Linq;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Event;

namespace FwaEu.Modules.MasterDataUserNotifications
{
	public class EntityChangeDectectionInitializer : IEventListenersInitializer
	{
		public void Initialize(EventListeners eventListeners, IScopedServiceProvider scopedServiceProvider)
		{
			var listener = Enumerable.Repeat(new EntityChangeDectection(scopedServiceProvider), 1);
			eventListeners.FlushEntityEventListeners = eventListeners.FlushEntityEventListeners.Concat(listener).ToArray();
		}
	}
}
