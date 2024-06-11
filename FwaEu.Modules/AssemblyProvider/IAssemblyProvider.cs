using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.AssemblyProvider
{
	public interface IAssemblyProvider
	{
		Assembly GetAssembly();
	}
}
