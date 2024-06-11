using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public interface IDataAccess
	{
		Task<object[]> GetAllAsync();
		Task<object> FindAsync(PropertyValueSet[] keys);
		Task SaveOrUpdateAsync(object model);
	}

	public interface IDataAccess<TModel> : IDataAccess
	{
		new Task<TModel[]> GetAllAsync();
		new Task<TModel> FindAsync(PropertyValueSet[] keys);
		Task SaveOrUpdateAsync(TModel model);
	}

	public abstract class DataAccess<TModel> : IDataAccess<TModel>
	{
		public abstract Task<TModel> FindAsync(PropertyValueSet[] keys);
		public abstract Task<TModel[]> GetAllAsync();
		public abstract Task SaveOrUpdateAsync(TModel model);

		async Task<object> IDataAccess.FindAsync(PropertyValueSet[] keys)
		{
			return await this.FindAsync(keys);
		}

		async Task<object[]> IDataAccess.GetAllAsync()
		{
			return (await this.GetAllAsync()).Cast<object>().ToArray();
		}

		Task IDataAccess.SaveOrUpdateAsync(object model)
		{
			return this.SaveOrUpdateAsync((TModel)model);
		}
	}
}
