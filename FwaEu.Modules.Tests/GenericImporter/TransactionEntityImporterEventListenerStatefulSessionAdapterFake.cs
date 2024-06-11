using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class TransactionSpy : ITransaction
	{
		public bool IsActive { get; private set; }
		public bool WasRolledBack { get; private set; }
		public bool WasCommitted { get; private set; }
		public bool IsDisposed { get; private set; }

		public TransactionSpy()
		{
			this.IsActive = true;
		}
		public Task CommitAsync()
		{
			this.IsActive = false;
			this.WasCommitted = true;
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			this.IsDisposed = true;
		}

		public Task RollbackAsync()
		{
			this.IsActive = false;
			this.WasRolledBack = true;
			return Task.CompletedTask;
		}
	}

	public class TransactionEntityImporterEventListenerStatefulSessionAdapterFake : IStatefulSessionAdapter
	{
		public object InnerSession => throw new NotImplementedException();

		//NOTE : https://books.google.fr/books?id=7olML0aUpVsC&lpg=PA1&hl=fr&pg=PT211#v=onepage&q&f=false
		public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			return new TransactionSpy();
		}
		public Task DeleteAsync(object entity)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			//NOTE: Nothing here
		}

		public Task FlushAsync()
		{
			return Task.CompletedTask;
		}

		public Task<TEntity> GetAsync<TEntity>(object id)
		{
			throw new NotImplementedException();
		}

		public Task<T> GetSequenceNextValueAsync<T>(string sequenceName)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> LoadAsync<TEntity>(object id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TEntity> Query<TEntity>()
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(object entity)
		{
			throw new NotImplementedException();
		}
	}
}
