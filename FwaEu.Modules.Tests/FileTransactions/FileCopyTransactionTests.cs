using FwaEu.Modules.FileTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.FileTransactions
{
	[TestClass]
	public class FileCopyTransactionTests
	{
		[TestMethod]
		public async Task Finished()
		{
			var transaction = new FileCopyTransaction(new IFileCopyProcess[] { });

#pragma warning disable CS0642 // Possible mistaken empty statement
			await using (transaction) ;
#pragma warning restore CS0642 // Possible mistaken empty statement

			Assert.IsTrue(transaction.Finished);
		}

		[TestMethod]
		public async Task Committed()
		{
			var transaction = new FileCopyTransaction(new IFileCopyProcess[] { });
			
			await using (transaction)
			{
				await transaction.CopyAllAsync(CancellationToken.None);
				await transaction.CommitAsync();
			}

			Assert.IsTrue(transaction.Committed);
		}

		[TestMethod]
		public async Task CommitAsync_CallProcessesCopyAsync()
		{
			var spy = new FileCopyProcessSpy();
			var transaction = new FileCopyTransaction(spy);

			await using (transaction)
			{
				await transaction.CopyAllAsync(CancellationToken.None);
			}

			Assert.IsTrue(spy.CopyAsyncInvoked);
		}

		[TestMethod]
		public async Task CommitAsync_CallProcessesDeleteExistingAsync()
		{
			var spy = new FileCopyProcessSpy();
			var transaction = new FileCopyTransaction(spy);

			await using (transaction)
			{
				await transaction.CopyAllAsync(CancellationToken.None); // NOTE: Required before commit
				await transaction.CommitAsync();
			}

			Assert.IsTrue(spy.DeleteExistingAsyncInvoked);
		}

		[TestMethod]
		public async Task RollbackAsync_CallProcessesDeleteNewAsync()
		{
			var spy = new FileCopyProcessSpy();
			var transaction = new FileCopyTransaction(spy);

			await using (transaction)
			{
				await transaction.RollbackAsync();
			}

			Assert.IsTrue(spy.DeleteNewAsyncInvoked);
		}

		[TestMethod]
		public async Task DisposeAsync_CallProcessesDisposeAsync()
		{
			var spy = new FileCopyProcessSpy();
			var transaction = new FileCopyTransaction(spy);

#pragma warning disable CS0642 // Possible mistaken empty statement
			await using (transaction) ;
#pragma warning restore CS0642 // Possible mistaken empty statement

			Assert.IsTrue(spy.DisposeAsyncInvoked);
		}
	}
}
