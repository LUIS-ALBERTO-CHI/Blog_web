using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public interface IEntityDataAccessFactory<TEntity> : IDataAccessFactory<TEntity>
		where TEntity : class
	{
	}

	public interface IEntityDataAccess<TEntity> : IDataAccess<TEntity>
		where TEntity : class
	{
	}

	public class EntityDataAccessFactory<TEntity> : IEntityDataAccessFactory<TEntity>
		where TEntity : class
	{
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly MainSessionContext _mainSessionContext;


		public EntityDataAccessFactory(IRepositoryFactory repositoryFactory,
			ISessionAdapterFactory sessionAdapterFactory,
			MainSessionContext mainSessionContext)
		{
			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));

			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			this._mainSessionContext = mainSessionContext
				?? throw new ArgumentNullException(nameof(mainSessionContext));
		}

		public virtual IDataAccess CreateDataAccess(ServiceStore serviceStore)
		{
			serviceStore.GetOrAdd<MainSessionContext>(() => this._mainSessionContext);
			var session = serviceStore.GetOrAdd<IStatefulSessionAdapter>(() => this._mainSessionContext.RepositorySession.Session);
			var repository = this._repositoryFactory.CreateByEntityType(typeof(TEntity), session);

			return new EntityDataAccess<TEntity>((IRepository<TEntity>)repository);
		}
	}

	public class EntityDataAccess<TEntity> : DataAccess<TEntity>, IEntityDataAccess<TEntity>
		where TEntity : class
	{
		public EntityDataAccess(IRepository<TEntity> repository)
		{
			this.Repository = repository
				?? throw new ArgumentNullException(nameof(repository));
		}

		public IRepository<TEntity> Repository { get; }

		public override async Task<TEntity[]> GetAllAsync()
		{
			return (await this.Repository.Query().ToListAsync()).ToArray();
		}

		public override Task<TEntity> FindAsync(PropertyValueSet[] keys)
		{
			if (keys.Length == 0)
			{
				throw new NotSupportedException();
			}

			var parameter = Expression.Parameter(typeof(TEntity), "x");
			var result = default(Expression);

			foreach (var propertyValueSet in keys)
			{
				if (propertyValueSet.Data.Any(d => d.Name != null))
				{
					//TODO: Handle the search by sub properties https://fwaeu.visualstudio.com/BlogWeb/_workitems/edit/4911/

					throw new NotSupportedException(
						"Not supported currently. If you need to use sub properties with database access, " +
						$"create a custom {typeof(IEntityDataAccess<TEntity>).Name}.");
				}

				var propertyValue = propertyValueSet.Data[0];

				var match = Expression.Equal(
					Expression.Property(parameter, propertyValueSet.PropertyName),
					Expression.Constant(propertyValue.Value, propertyValue.Type));

				result = result == null ? match : Expression.AndAlso(result, match);
			}

			var lambda = Expression.Lambda<Func<TEntity, bool>>(result, parameter);
			var q = this.Repository.Query().Where(lambda);

			return q.SingleOrDefaultAsync();
		}

		public override async Task SaveOrUpdateAsync(TEntity entity)
		{
			await this.Repository.SaveOrUpdateAsync(entity);
		}
	}
}
