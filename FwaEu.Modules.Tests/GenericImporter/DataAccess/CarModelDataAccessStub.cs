using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class CarModelDataAccessStub : IDataAccess<CarModel>
	{
		public CarModelDataAccessStub(List<CarModel> cars)
		{
			this._cars = cars ?? throw new ArgumentNullException(nameof(cars));
		}

		private readonly List<CarModel> _cars;

		public Task<CarModel> FindAsync(PropertyValueSet[] keys)
		{
			throw new NotImplementedException();
		}

		public Task<CarModel[]> GetAllAsync()
		{
			return Task.FromResult(this._cars.ToArray());
		}

		public Task SaveOrUpdateAsync(CarModel model)
		{
			if (model.Id == 0)
			{
				model.Id = this._cars.Max(c => c.Id) + 1;
				this._cars.Add(model);
			}

			return Task.CompletedTask;
		}

		public async Task SaveOrUpdateAsync(object model)
		{
			await this.SaveOrUpdateAsync((CarModel)model);
		}

		async Task<object> IDataAccess.FindAsync(PropertyValueSet[] keys)
		{
			return await this.FindAsync(keys);
		}

		async Task<object[]> IDataAccess.GetAllAsync()
		{
			return (await this.GetAllAsync()).Cast<object>().ToArray();
		}
	}
}
