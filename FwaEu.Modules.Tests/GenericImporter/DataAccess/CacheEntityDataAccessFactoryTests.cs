using FwaEu.Fwamework;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	[TestClass]
	public class CacheEntityDataAccessFactoryTests
	{
		[TestMethod]
		public void CreateDataAccess_MustUseProvidedUnderlyingDataAccessFactory()
		{
			var underlyingDataAccess = new EntityDataAccess<CarModel>(new CarModelRepositoryDummy());

			//NOTE: Fake factory actually returns the data access provided in constructor
			var underlyingFactory = new CarModelEntityDataAccessFactoryFake(underlyingDataAccess);

			var factory = new CacheEntityDataAccessFactory<CarModel>(underlyingFactory);
			using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped))
			{
				var dataAccess = factory.CreateDataAccess(serviceStore);
				Assert.IsInstanceOfType(dataAccess, typeof(CacheDataAccess<CarModel>));

				var cacheDataAccess = (CacheDataAccess<CarModel>)dataAccess;
				Assert.AreEqual(underlyingDataAccess, cacheDataAccess.UnderlyingDataAccess);
			}
		}
	}
}
