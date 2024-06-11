using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.GenericImporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter
{
	[TestClass]
	public class TransactionEntityImporterEventListenerTests
	{
		private enum TransactionState
		{
			Active,
			Committed,
			Rollback
		}

		private static void AssertTransactionState(TransactionSpy transaction, TransactionState state)
		{
			Assert.IsTrue(state == TransactionState.Active == transaction.IsActive);
			Assert.IsTrue(state == TransactionState.Committed == transaction.WasCommitted);
			Assert.IsTrue(state == TransactionState.Rollback == transaction.WasRolledBack);
			Assert.IsTrue(state != TransactionState.Active == transaction.IsDisposed);
		}

		private static ServiceStore CreateServiceStore()
		{
			var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped);
			serviceStore.Add<IStatefulSessionAdapter>(
					new TransactionEntityImporterEventListenerStatefulSessionAdapterFake());

			return serviceStore;
		}

		private async static Task SimulateSaveAsync(TransactionEntityImporterEventListener listener, object data)
		{
			await listener.OnModelSavingAsync(data);
			await listener.OnModelSavedAsync(data);
		}

		[TestMethod]
		public async Task OnModelSavingAsync_MustCreateTransaction()
		{
			using (var serviceStore = CreateServiceStore())
			{
				var listener = new TransactionEntityImporterEventListener(serviceStore);
				await SimulateSaveAsync(listener, null);

				var transaction = (TransactionSpy)serviceStore.Get<ITransaction>();
				Assert.IsNotNull(transaction);
				AssertTransactionState(transaction, TransactionState.Active);
			}
		}

		[TestMethod]
		public async Task OnModelSavedAsync_MustCommitTransaction()
		{
			using (var serviceStore = CreateServiceStore())
			{
				var listener = new TransactionEntityImporterEventListener(serviceStore);
				await SimulateSaveAsync(listener, 0); // NOTE: First fake insert to create the transaction and keep the reference to the instance

				var transaction = (TransactionSpy)serviceStore.Get<ITransaction>();

				foreach (var i in Enumerable.Range(1,
					TransactionEntityImporterEventListener.CommitTransactionAfterSavedItemCount - 1)) //NOTE: -1 because first fake insert previously
				{
					await SimulateSaveAsync(listener, i);
				}

				AssertTransactionState(transaction, TransactionState.Committed);
			}
		}

		[TestMethod]
		public async Task OnModelSavedAsync_MustCreateNewTransaction()
		{
			using (var serviceStore = CreateServiceStore())
			{
				var listener = new TransactionEntityImporterEventListener(serviceStore);
				await SimulateSaveAsync(listener, 0); // NOTE: First fake insert to create the transaction and keep the reference to the instance

				var transaction = serviceStore.Get<ITransaction>();

				foreach (var i in Enumerable.Range(1,
					TransactionEntityImporterEventListener.CommitTransactionAfterSavedItemCount)) //NOTE: -1 because first fake insert previously
				{
					await SimulateSaveAsync(listener, i);
				}

				var secondTransaction = serviceStore.Get<ITransaction>();
				Assert.AreNotEqual(transaction, secondTransaction);
			}
		}

		[TestMethod]
		public async Task OnImportFinished_MustCommitTransaction()
		{
			using (var serviceStore = CreateServiceStore())
			{
				var listener = new TransactionEntityImporterEventListener(serviceStore);
				await SimulateSaveAsync(listener, 0); // NOTE: First fake insert to create the transaction and keep the reference to the instance

				var transaction = (TransactionSpy)serviceStore.Get<ITransaction>();
				await listener.OnImportFinished();

				var transactionAfterImportFinished = serviceStore.Get<ITransaction>();

				AssertTransactionState(transaction, TransactionState.Committed);
				Assert.IsNull(transactionAfterImportFinished);
			}
		}

		[TestMethod]
		public async Task OnDisposeAsync_MustRollbackTransaction()
		{
			using (var serviceStore = CreateServiceStore())
			{
				var transaction = default(TransactionSpy);
				try
				{
					await using (var listener = new TransactionEntityImporterEventListener(serviceStore))
					{
						await SimulateSaveAsync(listener, 0); // NOTE: First fake insert to create the transaction and keep the reference to the instance
						transaction = (TransactionSpy)serviceStore.Get<ITransaction>();
						throw new Exception();
					}
				}
				catch
				{
					AssertTransactionState(transaction, TransactionState.Rollback);
				}
			}
		}
	}
}