using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IRepositoryBase
	{
		EntityDescriptor GetEntityDescriptor();
		void Configure(ISessionAdapter session, IServiceProvider serviceProvider);
	}

	public interface IRepository : IRepositoryBase
	{
		Task<object> GetAsync(object id);
		Task<object> GetNoPerimeterAsync(object id);

		Task SaveOrUpdateAsync(object entity);
		Task DeleteAsync(object entity);

		IQueryable QueryNoPerimeter();
		IQueryable Query();

		object CreateNew();
	}

	public interface IRepository<TEntity> : IRepositoryBase
		where TEntity : class
	{
		Task SaveOrUpdateAsync(TEntity entity);
		Task DeleteAsync(TEntity entity);

		IQueryable<TEntity> QueryNoPerimeter();
		IQueryable<TEntity> Query();

		TEntity CreateNew();
	}

	public interface IRepository<TEntity, TIdentifier> : IRepository<TEntity>
		where TEntity : class
	{
		Task<TEntity> GetAsync(TIdentifier id);
		Task<TEntity> GetNoPerimeterAsync(TIdentifier id);
	}
}
