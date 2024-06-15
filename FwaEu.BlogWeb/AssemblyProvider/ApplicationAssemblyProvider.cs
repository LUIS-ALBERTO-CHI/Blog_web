using FwaEu.Modules.AssemblyProvider;
using System.Reflection;

namespace FwaEu.BlogWeb.AssemblyProvider
{
	public class ApplicationAssemblyProvider : IAssemblyProvider
	{
		public Assembly GetAssembly()
		{
			return typeof(ApplicationAssemblyProvider).Assembly;
		}
	}
}
