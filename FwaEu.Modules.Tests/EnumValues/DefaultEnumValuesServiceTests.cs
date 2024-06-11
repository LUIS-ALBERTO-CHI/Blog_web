using FwaEu.Modules.AssemblyProvider;
using FwaEu.Modules.EnumValues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.EnumValues
{
	public enum BestEnum
	{
		Bad,
		Average,
		Good,
	}

	[TestClass]
	public class DefaultEnumValuesServiceTests
	{
		[TestMethod]
		public void GetEnumValues_ResultMatches()
		{
			var enumValuesService = new DefaultEnumValuesService(new List<IAssemblyProvider> { new FwaEuModulesTestsAssemblyProviderMock() });
			var enumType = typeof(BestEnum);
			var enumValuesFromService = enumValuesService.GetEnumValues(enumType.FullName).Values;
			var enumValues = Enum.GetValues(enumType).Cast<object>().Select(et => Convert.ToString(et));

			CollectionAssert.AreEquivalent(enumValuesFromService.ToArray(), enumValues.ToArray());
		}

		[TestMethod]
		public void GetEnumValues_EnumNotFound()
		{
			var enumValuesService = new DefaultEnumValuesService(new List<IAssemblyProvider> { new FwaEuModulesTestsAssemblyProviderMock() });

			Assert.ThrowsException<EnumNotFoundException>(() => enumValuesService.GetEnumValues("IDoNotExistEnum"));
		}
	}
}
