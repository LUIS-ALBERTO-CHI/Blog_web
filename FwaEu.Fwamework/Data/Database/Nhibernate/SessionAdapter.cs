using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public abstract class SessionAdapter<TNhibernateSession> : ISessionAdapter
			where TNhibernateSession : class, IDisposable
	{
		public SessionAdapter(TNhibernateSession nhibernateSession)
		{
			this.NhibernateSession = nhibernateSession ?? throw new ArgumentNullException(nameof(nhibernateSession));
		}

		public TNhibernateSession NhibernateSession { get; }

		public abstract Task<TEntity> GetAsync<TEntity>(object id);

		public abstract Task SaveOrUpdateAsync(object entity);
		public abstract Task DeleteAsync(object entity);

		public abstract IQueryable<TEntity> Query<TEntity>();

		public virtual void Dispose()
		{
			this.NhibernateSession.Dispose();
		}

		public abstract ITransaction BeginTransaction(IsolationLevel isolationLevel);
		public abstract Dialect GetDialect();
		public abstract NHibernate.ISQLQuery CreateSQLQuery(string query);

		public virtual async Task<T> GetSequenceNextValueAsync<T>(string sequenceName)
		{
			var dialect = this.GetDialect();
			var query = dialect.GetSequenceNextValString(sequenceName);

			return (T)Convert.ChangeType(await this.CreateSQLQuery(query).UniqueResultAsync(), typeof(T), CultureInfo.InvariantCulture);
		}


		object ISessionAdapter.InnerSession => this.NhibernateSession;
	}
}
