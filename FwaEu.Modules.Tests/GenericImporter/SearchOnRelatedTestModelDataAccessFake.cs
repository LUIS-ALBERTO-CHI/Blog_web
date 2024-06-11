using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class SearchOnRelatedTestModelDataAccessFakeFactory : IDataAccessFactory<SearchOnRelatedTestModel>
	{
		public SearchOnRelatedTestModelDataAccessFakeFactory(SearchOnRelatedTestModel[] models)
		{
			this._models = models ?? throw new ArgumentNullException(nameof(models));
		}

		private readonly SearchOnRelatedTestModel[] _models;

		public IDataAccess CreateDataAccess(ServiceStore serviceStore)
		{
			return new SearchOnRelatedTestModelDataAccessFake(this._models);
		}
	}

	public class SearchOnRelatedTestModelDataAccessFake : IDataAccess<SearchOnRelatedTestModel>
	{
		public SearchOnRelatedTestModelDataAccessFake(SearchOnRelatedTestModel[] models)
		{
			this._models = models ?? throw new ArgumentNullException(nameof(models));
		}

		private readonly SearchOnRelatedTestModel[] _models;

		public Task<SearchOnRelatedTestModel> FindAsync(PropertyValueSet[] keys)
		{
			if (keys.Length != 2)
			{
				throw new NotSupportedException();
			}

			var name = (string)keys.First(k => k.PropertyName == nameof(SearchOnRelatedTestModel.Name)).Data[0].Value;
			var age = Convert.ToInt32((string)keys.First(k => k.PropertyName == nameof(SearchOnRelatedTestModel.Age)).Data[0].Value);

			return Task.FromResult(this._models
				.SingleOrDefault(model => model.Name == name && model.Age == age));
		}

		public Task<SearchOnRelatedTestModel[]> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(SearchOnRelatedTestModel model)
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(object model)
		{
			throw new NotImplementedException();
		}

		async Task<object> IDataAccess.FindAsync(PropertyValueSet[] keys)
		{
			return await this.FindAsync(keys);
		}

		Task<object[]> IDataAccess.GetAllAsync()
		{
			throw new NotImplementedException();
		}
	}
}
