using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public class CacheEntityDataAccessFactory<TEntity> : IDataAccessFactory<TEntity>
			where TEntity : class
	{
		public CacheEntityDataAccessFactory(IEntityDataAccessFactory<TEntity> entityDataAccessFactory)
		{
			this._entityDataAccessFactory = entityDataAccessFactory;
		}

		private readonly IEntityDataAccessFactory<TEntity> _entityDataAccessFactory;

		public virtual IDataAccess CreateDataAccess(ServiceStore serviceStore)
		{
			var dataAccess = this._entityDataAccessFactory.CreateDataAccess(serviceStore);
			return new CacheDataAccess<TEntity>((IDataAccess<TEntity>)dataAccess);
		}
	}

	public class CacheDataAccess<TModel> : DataAccess<TModel>
	{
		public CacheDataAccess(IDataAccess<TModel> underlyingDataAccess)
		{
			this.UnderlyingDataAccess = underlyingDataAccess
				?? throw new ArgumentNullException(nameof(underlyingDataAccess));

			this.Cache = new ModelCache<TModel>(this.UnderlyingDataAccess);
		}

		protected ModelCache<TModel> Cache { get; }
		public IDataAccess<TModel> UnderlyingDataAccess { get; }

		public override async Task<TModel> FindAsync(PropertyValueSet[] keys)
		{
			return await this.Cache.FindAsync(keys);
		}

		public override async Task<TModel[]> GetAllAsync()
		{
			return await this.Cache.GetAllAsync();
		}

		public async override Task SaveOrUpdateAsync(TModel model)
		{
			await this.UnderlyingDataAccess.SaveOrUpdateAsync(model);
			this.Cache.Add(model);
		}
	}
}
