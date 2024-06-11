using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate;
using NHibernate.Dialect;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface INhibernateStatefulSessionAdapter : IStatefulSessionAdapter
	{
		ISession NhibernateSession { get; }
		Task<T> GetSequenceNextValueAsync<T>(string sequenceName);
	}

	public class StatefulSessionAdapter : SessionAdapter<ISession>, INhibernateStatefulSessionAdapter
	{
		public StatefulSessionAdapter(ISession nhibernateSession)
			: base(nhibernateSession)
		{
		}

		public override async Task<TEntity> GetAsync<TEntity>(object id)
		{
			return await this.NhibernateSession.GetAsync<TEntity>(id);
		}

		public override async Task SaveOrUpdateAsync(object entity)
		{
			await this.NhibernateSession.SaveOrUpdateAsync(entity);
		}

		public override async Task DeleteAsync(object entity)
		{
			await this.NhibernateSession.DeleteAsync(entity);
		}

		public override IQueryable<TEntity> Query<TEntity>()
		{
			return this.NhibernateSession.Query<TEntity>();
		}

		public async Task FlushAsync()
		{
			await this.NhibernateSession.FlushAsync();
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
