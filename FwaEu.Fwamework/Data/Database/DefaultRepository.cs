using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.Fwamework.Data.Database
{
	public abstract class DefaultRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>, IRepository
		where TEntity : class
	{
		protected ISessionAdapter Session { get; private set; }
		protected IServiceProvider ServiceProvider { get; private set; }

		protected virtual IEnumerable<IRepositoryDataFilter<TEntity, TIdentifier>> CreateDataFilters(
			RepositoryDataFilterContext<TEntity, TIdentifier> context)
		{
			return Enumerable.Empty<IRepositoryDataFilter<TEntity, TIdentifier>>();
		}

		protected RepositoryDataFilterContext<TEntity, TIdentifier> CreateDataFilterContext()
		{
			return new RepositoryDataFilterContext<TEntity, TIdentifier>(this, this.Session, this.ServiceProvider);
		}

		EntityDescriptor IRepositoryBase.GetEntityDescriptor()
		{
			return new EntityDescriptor(typeof(TEntity), typeof(TIdentifier));
		}

		void IRepositoryBase.Configure(ISessionAdapter session, IServiceProvider serviceProvider)
		{
			this.Session = session ?? throw new ArgumentNullException(nameof(session));
			this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public async Task<TEntity> GetAsync(TIdentifier id)
		{
			var entity = await this.GetNoPerimeterAsync(id);
			var context = this.CreateDataFilterContext();

			foreach (var filter in this.CreateDataFilters(context))
			{
				var accept = await filter.AcceptAsync(entity, context);
				if (!accept)
				{
					return null;
				}
			}

			return entity;
		}

		public virtual async Task<TEntity> GetNoPerimeterAsync(TIdentifier id)
		{
			return await this.Session.GetAsync<TEntity>(id);
		}

		public virtual async Task SaveOrUpdateAsync(TEntity entity)
		{
			await this.Session.SaveOrUpdateAsync(entity);
		}

		public virtual async Task DeleteAsync(TEntity entity)
		{
			await this.Session.DeleteAsync(entity);
		}

		public virtual IQueryable<TEntity> QueryNoPerimeter()
		{
			return this.Session.Query<TEntity>();
		}

		protected IQueryable<TEntity> ApplyDataFilters(IQueryable<TEntity> query,
			Func<RepositoryDataFilterContext<TEntity, TIdentifier>, IEnumerable<IRepositoryDataFilter<TEntity, TIdentifier>>> dataFilterProvider)
		{
			var context = this.CreateDataFilterContext();
			var filters = dataFilterProvider(context);

			foreach (var filter in filters)
			{
				var predicate = filter.Accept(context);
				if (predicate != null)
				{
					query = query.Where(predicate);
				}
			}

			return query;
		}

		public IQueryable<TEntity> Query()
		{
			return this.ApplyDataFilters(this.QueryNoPerimeter(),
				context => this.CreateDataFilters(context));
		}

		public virtual TEntity CreateNew()
		{
			return (TEntity)Activator.CreateInstance(typeof(TEntity), true);
		}

		async Task<object> IRepository.GetAsync(object id)
		{
			return await this.GetAsync((TIdentifier)id);
		}

		async Task<object> IRepository.GetNoPerimeterAsync(object id)
		{
			return await this.GetNoPerimeterAsync((TIdentifier)id);
		}

		Task IRepository.SaveOrUpdateAsync(object entity)
		{
			return this.SaveOrUpdateAsync((TEntity)entity);
		}

		Task IRepository.DeleteAsync(object entity)
		{
			return this.DeleteAsync((TEntity)entity);
		}

		IQueryable IRepository.QueryNoPerimeter()
		{
			return this.QueryNoPerimeter();
		}

		IQueryable IRepository.Query()
		{
			return this.Query();
		}

		object IRepository.CreateNew()
		{
			return this.CreateNew();
		}
	}
}
