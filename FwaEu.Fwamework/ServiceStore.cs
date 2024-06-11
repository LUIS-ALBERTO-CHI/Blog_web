using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework
{
	public enum ServiceStoreInnerServicesLifetime
	{
		/// <summary>
		/// Provided instances will be disposed when service store is disposed.
		/// </summary>
		Scoped,
		
		/// <summary>
		/// Provided instances will not be lost when service store is disposed.
		/// </summary>
		Singleton
	}

	public class ServiceStore : IDisposable
	{
		public ServiceStore(ServiceStoreInnerServicesLifetime innerServicesLifetime)
		{
			this.InnerServicesLifetime = innerServicesLifetime;
		}

		public ServiceStoreInnerServicesLifetime InnerServicesLifetime { get; }
		private Dictionary<Type, object> _services = new Dictionary<Type, object>();

		public TService Get<TService>()
		{
			return this._services.ContainsKey(typeof(TService)) ? (TService)this._services[typeof(TService)] : default(TService);
		}

		public TService GetOrAdd<TService>(Func<TService> factory)
		{
			if (!this._services.ContainsKey(typeof(TService)))
			{
				this._services.Add(typeof(TService), factory());
			}

			return (TService)this._services[typeof(TService)];
		}

		public void Add<TService>(TService instance)
		{
			this._services.Add(typeof(TService), instance);
		}

		public void Remove<TService>()
		{
			this._services.Remove(typeof(TService));
		}

		public void Dispose()
		{
			if (this.InnerServicesLifetime == ServiceStoreInnerServicesLifetime.Scoped)
			{
				foreach (var disposable in this._services.Values.OfType<IDisposable>())
				{
					disposable.Dispose();
				}
			}

			this._services = null;
		}
	}
}
