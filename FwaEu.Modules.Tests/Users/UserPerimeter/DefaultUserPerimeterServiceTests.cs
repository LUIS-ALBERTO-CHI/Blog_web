using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Users.UserPerimeter
{
	[TestClass]
	public class DefaultUserPerimeterServiceTests
	{
		[TestMethod]
		public async Task GetFullAccessPerimeterKeysAsync()
		{
			using (var serviceProvider = new ServiceCollection().BuildServiceProvider())
			{
				var countriesFactory = new CountriesUserPerimeterProviderStubFactory();
				var profilesFactory = new ProfilesUserPerimeterProviderStubFactory();

				var cache = new UserPerimeterProviderFactoryCache(
					new IUserPerimeterProviderFactory[] { countriesFactory, profilesFactory });

				var service = new DefaultUserPerimeterService(cache, serviceProvider);

				var fullAccesses = await service.GetFullAccessPerimeterKeysAsync(1); //NOTE: userId is ignored in stub implementations
				Assert.AreEqual(1, fullAccesses.Length);

				var fullAccess = fullAccesses.Single();
				Assert.AreEqual(fullAccess, profilesFactory.Key);
			}
		}

		[TestMethod]
		public async Task GetAccessesAsync()
		{
			using (var serviceProvider = new ServiceCollection().BuildServiceProvider())
			{
				var countriesFactory = new CountriesUserPerimeterProviderStubFactory();
				var profilesFactory = new ProfilesUserPerimeterProviderStubFactory();
				var factories = new IUserPerimeterProviderFactory[] { countriesFactory, profilesFactory };

				var cache = new UserPerimeterProviderFactoryCache(factories);
				var service = new DefaultUserPerimeterService(cache, serviceProvider);

				var accesses = await service.GetAccessesAsync(1); //NOTE: userId is ignored in stub implementations
				Assert.AreEqual(factories.Length, accesses.Length);

				var fullAccesses = accesses.Where(a => a.HasFullAccess).ToArray();
				Assert.AreEqual(1, fullAccesses.Length); // Profiles

				var fullAccess = fullAccesses.Single();
				Assert.IsNull(fullAccess.AccessibleIds);
				Assert.AreEqual(profilesFactory.Key, fullAccess.Key);

				var restrictedAccesses = accesses.Where(a => !a.HasFullAccess).ToArray();
				Assert.AreEqual(1, restrictedAccesses.Length); // Countries

				var restrictedAccess = restrictedAccesses.Single();
				Assert.IsNotNull(restrictedAccess.AccessibleIds);
				Assert.AreEqual(countriesFactory.Key, restrictedAccess.Key);
			}
		}
	}
}
