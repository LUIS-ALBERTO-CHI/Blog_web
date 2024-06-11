using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface INhibernateStatelessSessionAdapter : IStatelessSessionAdapter
	{
		IStatelessSession NhibernateSession { get; }
	}

	public class StatelessSessionAdapter : SessionAdapter<IStatelessSession>, INhibernateStatelessSessionAdapter
	{
		public StatelessSessionAdapter(IStatelessSession nhibernateSession)
			: base(nhibernateSession)
		{
		}

		public override async Task<TEntity> GetAsync<TEntity>(object id)
		{
			return await this.NhibernateSession.GetAsync<TEntity>(id);
		}

		public override async Task SaveOrUpdateAsync(object entity)
		{
			if (entity is IEntity castedEntity)
			{
				if (castedEntity.IsNew())
				{
					await this.NhibernateSession.InsertAsync(entity);
					return;
				}

				await this.NhibernateSession.UpdateAsync(entity);
				return;
			}

			throw new NotSupportedException(
				$"To use stateless session adapter with entity which is not implementing '{typeof(IEntity).FullName}', you have to implements a custom adapter.");
		}

		public override async Task DeleteAsync(object entity)
		{
			await this.NhibernateSession.DeleteAsync(entity);
		}

		public override IQueryable<TEntity> Query<TEntity>()
		{
			return this.NhibernateSession.Query<TEntity>();
		}

		//NOTE : https://books.google.fr/books?id=7olML0aUpVsC&lpg=PA1&hl=fr&pg=PT211#v=onepage&q&f=false
		public override Sessions.ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			return new NhibernateTransactionAdapter(this.NhibernateSession.BeginTransaction(isolationLevel));
		}

		public override Dialect GetDialect()
		{
			return this.NhibernateSession.GetSessionImplementation().Factory.Dialect;
		}

		public override ISQLQuery CreateSQLQuery(string query)
		{
			return this.NhibernateSession.CreateSQLQuery(query);
		}
	}
}
