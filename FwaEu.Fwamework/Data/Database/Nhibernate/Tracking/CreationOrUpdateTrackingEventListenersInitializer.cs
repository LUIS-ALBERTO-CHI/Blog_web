using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.DependencyInjection;
using NHibernate.Event;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Tracking
{
	public class CreationOrUpdateTrackingEventListenersInitializer : IEventListenersInitializer
	{
		public void Initialize(EventListeners eventListeners, IScopedServiceProvider scopedServiceProvider)
		{
			var listener = Enumerable.Repeat(new CreationOrUpdateTrackingEventListener(scopedServiceProvider), 1);

			eventListeners.PreInsertEventListeners = eventListeners.PreInsertEventListeners.Concat(listener).ToArray();
			eventListeners.PreUpdateEventListeners = eventListeners.PreUpdateEventListeners.Concat(listener).ToArray();
		}
	}
}
