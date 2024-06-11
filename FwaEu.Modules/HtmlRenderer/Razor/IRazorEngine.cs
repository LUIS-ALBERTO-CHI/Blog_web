using Microsoft.Extensions.Hosting;
using System.Dynamic;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public interface IRazorEngine : IHostedService
	{
		Task<string> CompileRenderStringAsync<T>(string key, string content, T model, ExpandoObject viewBag = null);
	}
}