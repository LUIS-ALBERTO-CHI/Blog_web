using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IParametersProvider
	{
		Task<Dictionary<string, object>> LoadAsync();
	}
}
