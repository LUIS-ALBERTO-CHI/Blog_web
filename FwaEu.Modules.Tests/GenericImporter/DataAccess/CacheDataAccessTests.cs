using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	[TestClass]
	public class CacheDataAccessTests
	{
		[TestMethod]
		public async Task SaveOrUpdateAsync_AddModelToCache()
		{
			var cars = new List<CarModel>()
			{
				new CarModel() { Id = 1, Color = "Blue", Brand = "Citroen" },
				new CarModel() { Id = 2, Color = "Orange", Brand = "Renault" },
			};

			var initialCount = cars.Count;
			var carModelDataAccessStub = new CarModelDataAccessStub(cars);
			var cacheDataAccess = new CacheDataAccess<CarModel>(carModelDataAccessStub);

			var carsBeforeSave = await cacheDataAccess.GetAllAsync();
			Assert.AreEqual(cars.Count, carsBeforeSave.Length);

			var newCar = new CarModel() { Color = "New" };

			await cacheDataAccess.SaveOrUpdateAsync(newCar);
			Assert.AreEqual(initialCount + 1, cars.Count);
			Assert.IsTrue(cars.Contains(newCar));

			var carsAfterSave = await cacheDataAccess.GetAllAsync();
			Assert.AreEqual(cars.Count, carsAfterSave.Length);
		}

		[TestMethod]
		public async Task FindAsync()
		{
			var carToSearch = new CarModel() { Id = 1, Color = "Blue", Brand = "Citroen" };
			var cars = new List<CarModel>()
			{
				carToSearch,
				new CarModel() { Id = 2, Color = "Orange", Brand = "Renault" },
			};

			var carModelDataAccessStub = new CarModelDataAccessStub(cars);
			var cacheDataAccess = new CacheDataAccess<CarModel>(carModelDataAccessStub);

			var keys = new[]
			{
				new PropertyValueSet(carToSearch.Color, nameof(CarModel.Color), typeof(string)),
			};

			var carSearched = await cacheDataAccess.FindAsync(keys);
			Assert.AreEqual(carToSearch, carSearched);
		}

		[TestMethod]
		public async Task FindAsync_AfterSave()
		{
			var cars = new List<CarModel>()
			{
				new CarModel() { Id = 1, Color = "Blue", Brand = "Citroen" },
				new CarModel() { Id = 2, Color = "Orange", Brand = "Renault" },
			};

			var carModelDataAccessStub = new CarModelDataAccessStub(cars);
			var cacheDataAccess = new CacheDataAccess<CarModel>(carModelDataAccessStub);

			var newCar = new CarModel() { Id = 1, Color = "Yellow", Brand = "Nissan" };

			var keys = new[]
			{
				new PropertyValueSet(newCar.Color, nameof(CarModel.Color), typeof(string)),
				new PropertyValueSet(newCar.Brand, nameof(CarModel.Brand), typeof(string)),
			};

			var carSearchedBeforeSave = await cacheDataAccess.FindAsync(keys);
			Assert.IsNull(carSearchedBeforeSave);

			await cacheDataAccess.SaveOrUpdateAsync(newCar);

			var carSearchedAfterSave = await cacheDataAccess.FindAsync(keys);
			Assert.IsNotNull(carSearchedAfterSave);
		}
	}
}
