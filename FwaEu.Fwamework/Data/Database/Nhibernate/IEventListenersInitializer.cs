using FwaEu.Fwamework.DependencyInjection;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface IEventListenersInitializer
	{
		void Initialize(EventListeners eventListeners,
			IScopedServiceProvider scopedServiceProvider);
	}
}
