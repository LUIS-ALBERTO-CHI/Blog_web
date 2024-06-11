using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public class ModelCache<TModel>
	{
		public ModelCache(IDataAccess<TModel> dataAccess)
		{
			this.DataAccess = dataAccess
				?? throw new ArgumentNullException(nameof(dataAccess));
		}

		private List<TModel> _list;

		private readonly Dictionary<string, IModelCacheByPropertyCombination<TModel>> _caches
			   = new Dictionary<string, IModelCacheByPropertyCombination<TModel>>();

		protected IDataAccess<TModel> DataAccess { get; }

		protected virtual IModelCacheByPropertyCombination<TModel> CreateModelCacheByPropertyCombination(
			List<TModel> models, PropertyValueSet[] keys)
		{
			return new StringKeyModelCacheByPropertyCombination<TModel>(models,
				keys, StringComparer.InvariantCultureIgnoreCase);
		}

		public async Task<TModel> FindAsync(PropertyValueSet[] keys)
		{
			var storeKey = String.Join('|',
				keys.Select(k => k.Data.Length == 1 && k.Data[0].Name == null
				? k.PropertyName
				: "[" + String.Join(", ", k.Data.Select(d => $"{k.PropertyName}.{d.Name}")) + "]"));

			if (!this._caches.ContainsKey(storeKey))
			{
				if (this._list == null) //NOTE: Same code as in GetAllAsync(), but not factorized, to minimize use of await
				{
					this._list = (await this.DataAccess.GetAllAsync()).ToList();
				}

				this._caches.Add(storeKey, this.CreateModelCacheByPropertyCombination(this._list, keys));
			}

			var cache = this._caches[storeKey];
			return cache.Find(keys);
		}

		public async Task<TModel[]> GetAllAsync()
		{
			if (this._list == null) //NOTE: Same code as in FindAsync(), but not factorized, to minimize use of await
			{
				this._list = (await this.DataAccess.GetAllAsync()).ToList();
			}

			return this._list.ToArray();
		}

		public void Add(TModel model)
		{
			this._list.Add(model);

			foreach (var cache in this._caches.Values)
			{
				cache.Add(model);
			}
		}
	}
}
