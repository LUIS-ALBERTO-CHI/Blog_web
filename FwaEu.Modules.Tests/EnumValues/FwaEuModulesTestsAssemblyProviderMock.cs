using FwaEu.Modules.AssemblyProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.EnumValues
{
	public class FwaEuModulesTestsAssemblyProviderMock : IAssemblyProvider
	{
		public Assembly GetAssembly()
		{
			return typeof(FwaEuModulesTestsAssemblyProviderMock).Assembly;
		}
	}
}
