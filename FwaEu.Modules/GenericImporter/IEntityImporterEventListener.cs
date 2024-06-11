using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.Modules.GenericImporter
{
	public class EntityImporterEventListenerFactory : IModelImporterEventListenerFactory<IEntityImporterEventListener>
	{
		public IEntityImporterEventListener Create(ServiceStore serviceStore, IServiceProvider serviceProvider)
		{
			return new TransactionEntityImporterEventListener(serviceStore);
		}
	}

	public interface IEntityImporterEventListener : IModelImporterEventListener
	{
	}

	public class TransactionEntityImporterEventListener : EmptyModelImporterEventListener, IEntityImporterEventListener
	{
		private enum ClearAction
		{
			Commit,
			Rollback
		}

		public TransactionEntityImporterEventListener(ServiceStore serviceStore)
		{
			this._serviceStore = serviceStore ?? throw new ArgumentNullException(nameof(serviceStore));
		}

		public const int CommitTransactionAfterSavedItemCount = 50; //NOTE: Value from previous Template and never modified

		private readonly ServiceStore _serviceStore;
		protected int CurrentTransactionSavedItemCount = 0;

		public override async Task OnModelSavingAsync(object model)
		{
			_ = GetTransaction(this._serviceStore, createWhenNotFound: true);
			await base.OnModelSavingAsync(model);
		}

		public override Task OnModelSavedAsync(object model)
		{
			if (++this.CurrentTransactionSavedItemCount == CommitTransactionAfterSavedItemCount)
			{
				return this.ClearTransactionAsync(ClearAction.Commit);
			}

			return Task.CompletedTask;
		}

		public override Task OnModelSavingErrorAsync(Exception exception)
		{
			((NHibernate.ISession)this._serviceStore.Get<IStatefulSessionAdapter>().InnerSession).Clear();
			return Task.CompletedTask;
		}

		public override async Task OnImportFinished()
		{
			await this.ClearTransactionAsync(ClearAction.Commit);
		}

		public override ValueTask DisposeAsync()
		{
			return new ValueTask(this.ClearTransactionAsync(ClearAction.Rollback));
		}

		private static ITransaction GetTransaction(ServiceStore serviceStore, bool createWhenNotFound)
		{
			var transaction = serviceStore.Get<ITransaction>();
			if (transaction == null && createWhenNotFound)
			{
				var session = serviceStore.Get<IStatefulSessionAdapter>();
				transaction = session.BeginTransaction();
				serviceStore.Add(transaction);
			}

			return transaction;
		}

		private async Task ClearTransactionAsync(ClearAction action)
		{
			var transaction = GetTransaction(this._serviceStore, createWhenNotFound: false);

			if (transaction != null)
			{
				if (action == ClearAction.Commit)
				{
					await this._serviceStore.Get<IStatefulSessionAdapter>().FlushAsync();
					await transaction.CommitAsync();
				}
				else
				{
					await transaction.RollbackAsync();
				}

				this._serviceStore.Remove<ITransaction>();
				transaction.Dispose();
			}

			this.CurrentTransactionSavedItemCount = 0;
		}
	}
}
