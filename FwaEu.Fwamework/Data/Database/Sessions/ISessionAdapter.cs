using FwaEu.Fwamework.Data;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public interface ISessionAdapter : IDisposable
	{
		Task<TEntity> GetAsync<TEntity>(object id);

		Task SaveOrUpdateAsync(object entity);
		Task DeleteAsync(object entity);

		IQueryable<TEntity> Query<TEntity>();

		object InnerSession { get; }

		//NOTE : https://books.google.fr/books?id=7olML0aUpVsC&lpg=PA1&hl=fr&pg=PT211#v=onepage&q&f=false
		ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

		Task<T> GetSequenceNextValueAsync<T>(string sequenceName);
	}

	public interface IStatefulSessionAdapter : ISessionAdapter
	{
		Task FlushAsync();
	}

	public interface IStatelessSessionAdapter : ISessionAdapter
	{

	}

	public interface ITransaction : IDisposable
	{
		Task CommitAsync();
		Task RollbackAsync();
	}

	public class NhibernateTransactionAdapter : ITransaction
	{
		private readonly NHibernate.ITransaction _transaction;

		public NhibernateTransactionAdapter(NHibernate.ITransaction transaction)
		{
			_transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
		}

		public async Task CommitAsync()
		{
			await _transaction.CommitAsync();
		}

		public async Task RollbackAsync()
		{
			await _transaction.RollbackAsync();
		}

		public void Dispose()
		{
			_transaction.Dispose();
		}
	}
}
