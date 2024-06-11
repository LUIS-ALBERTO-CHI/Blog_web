using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	[TestClass]
	public class StringKeyModelCacheByPropertyCombinationTests
	{
		[TestMethod]
		public void Find_OneProperty()
		{
			var carToSearch = new CarModel() { Id = 1, Color = "Red" };
			var cars = new[]
			{
				carToSearch,
				new CarModel() { Id = 2, Color = "Blue" },
				new CarModel() { Id = 3, Color = "Orange" },
			}
			.ToList();

			var keys = new[] { new PropertyValueSet(carToSearch.Color, nameof(CarModel.Color), typeof(string)) };

			var cache = new StringKeyModelCacheByPropertyCombination<CarModel>(
				cars, keys, StringComparer.InvariantCultureIgnoreCase);

			var result = cache.Find(keys);

			Assert.AreEqual(carToSearch, result);
		}

		[TestMethod]
		public void Find_ManyProperties()
		{
			var carToSearch = new CarModel() { Id = 1, Color = "Red", Brand = "Peugeot" };
			var cars = new[]
			{
				carToSearch,
				new CarModel() { Id = 2, Color = "Blue", Brand = "Citroen" },
				new CarModel() { Id = 3, Color = "Orange", Brand = "Renault" },
			}
			.ToList();

			var keys = new[] {
				new PropertyValueSet(carToSearch.Color, nameof(CarModel.Color), typeof(string)),
				new PropertyValueSet(carToSearch.Brand, nameof(CarModel.Brand), typeof(string))
			};

			var cache = new StringKeyModelCacheByPropertyCombination<CarModel>(
				cars, keys, StringComparer.InvariantCultureIgnoreCase);

			var result = cache.Find(keys);

			Assert.AreEqual(carToSearch, result);
		}

		[TestMethod]
		public void Find_DecimalStoreValueFormat_MustFail()
		{
			//NOTE: 10.000m.ToString() != 10.00000m.ToString()

			var firstCarPrice = 10.000m;

			var cars = new[]
			{
				new CarModel() { Id = 1, Color = "Blue", Price = firstCarPrice },
				new CarModel() { Id = 2, Color = "Orange", Price = 10.00000m },
			}
			.ToList();

			var keys = new[] { new PropertyValueSet(firstCarPrice, nameof(CarModel.Price), typeof(decimal)) };

			var cache = new StringKeyModelCacheByPropertyCombination<CarModel>(
				cars, keys, StringComparer.InvariantCultureIgnoreCase);

			Assert.ThrowsException<ArgumentException>(() =>
			{
				_ = cache.Find(keys);
			});
		}
	}
}
