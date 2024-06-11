using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework.DependencyInjection;
using NHibernate.Event;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Data.Database.Tracking;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Tracking
{
	public class CreationOrUpdateTrackingEventListener :
		NHibernate.Event.IPreUpdateEventListener,
		NHibernate.Event.IPreInsertEventListener
	{
		public CreationOrUpdateTrackingEventListener(IScopedServiceProvider scopedServiceProvider)
		{
			this._scopedServiceProvider = scopedServiceProvider
				?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}

		private readonly IScopedServiceProvider _scopedServiceProvider;

		private (IServiceProvider, UserEntity) GetServiceProviderAndCurrentUser()
		{
			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var currentUser = serviceProvider.GetService<ICurrentUserService>().User?.Entity;
			return (serviceProvider, currentUser);
		}

		private void SetProperties(bool inserting, AbstractPreDatabaseOperationEvent @event, object[] state)
		{
			var (serviceProvider, currentUser) = this.GetServiceProviderAndCurrentUser();
			var serviceDisabler = serviceProvider.GetRequiredService<Disabler>();
			if (serviceDisabler.IsDisabled())
				return;
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var entity = @event.Entity;

			void SetState(string name, object value)
			{
				var propertyIndex = Array.IndexOf(@event.Persister.PropertyNames, name);
				state[propertyIndex] = value;
			}

			if (inserting)
			{
				if (entity is ICreatedByTracked cb)
				{
					cb.CreatedBy = currentUser;
					SetState(nameof(cb.CreatedBy), currentUser);
				}

				if (entity is ICreatedOnTracked co)
				{
					co.CreatedOn = now;
					SetState(nameof(co.CreatedOn), now);
				}
			}

			if (entity is IUpdatedByTracked ub)
			{
				ub.UpdatedBy = currentUser;
				SetState(nameof(ub.UpdatedBy), currentUser);
			}
			if (entity is IUpdatedOnTracked uo)
			{
				uo.UpdatedOn = now;
				SetState(nameof(uo.UpdatedOn), now);
			}
		}

		public bool OnPreInsert(PreInsertEvent @event)
		{
			this.SetProperties(true, @event, @event.State);
			return false;
		}

		public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
		{
			this.SetProperties(true, @event, @event.State);
			return Task.FromResult(false);
		}

		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			this.SetProperties(false, @event, @event.State);
			return false;
		}

		public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
		{
			this.SetProperties(false, @event, @event.State);
			return Task.FromResult(false);
		}

		public class Disabler
		{
			private int DisableCount = 0;

			public IDisposable Disable()
			{
				return new DisablerState(this);
			}

			public bool IsDisabled() => DisableCount > 0;

			private class DisablerState : IDisposable
			{
				private Disabler Disabler;
				private bool Disposed = false;

				public DisablerState(Disabler disabler)
				{
					Disabler = disabler;
					++Disabler.DisableCount;
				}

				public void Dispose()
				{
					if (!Disposed)
					{
						Disposed = true;
						--Disabler.DisableCount;
					}
				}
			}
		}
	}
}