using FwaEu.Fwamework;
using FwaEu.Fwamework.Reflection;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FwaEu.Fwamework.Tests.Reflection
{
	[TestClass]
	public class ReflectionCacheTests
	{
		[TestMethod]
		public void GetPropertyTypes()
		{
			var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped);

			var testType = typeof(ReflectionCacheModelDummy);
			var reflectionType = TypeDescriptor.GetProperties(testType)
				.Cast<PropertyDescriptor>()
				.Count();
			var reflectionCacheWhenNotExistingInServiceStore = serviceStore.GetOrAdd<ReflectionCache>()
				.GetPropertyTypes(testType);

			Assert.AreEqual(reflectionType, reflectionCacheWhenNotExistingInServiceStore.Count());

			var reflectionCacheWhenAlreadyExistingInServiceStore = serviceStore.GetOrAdd<ReflectionCache>()
				.GetPropertyTypes(testType);

			Assert.AreEqual(reflectionCacheWhenNotExistingInServiceStore,
				reflectionCacheWhenAlreadyExistingInServiceStore);
		}
	}
}
