using FwaEu.Modules.AssemblyProvider;
using System.Reflection;

namespace FwaEu.TemplateCore.AssemblyProvider
{
	public class ApplicationAssemblyProvider : IAssemblyProvider
	{
		public Assembly GetAssembly()
		{
			return typeof(ApplicationAssemblyProvider).Assembly;
		}
	}
}
