using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.AssemblyProvider
{
	public class FwaEuModulesAssemblyProvider : IAssemblyProvider
	{
		public Assembly GetAssembly()
		{
			return typeof(FwaEuModulesAssemblyProvider).Assembly;
		}
	}
}
