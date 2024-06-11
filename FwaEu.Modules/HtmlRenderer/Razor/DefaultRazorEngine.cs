using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RazorLight;
using System.Dynamic;
using System.Threading.Tasks;
using System.Threading;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public class DefaultRazorEngine : IRazorEngine
	{
		private readonly RazorLightEngine Engine;
		private readonly bool Preload;

		public DefaultRazorEngine(IHostEnvironment hostEnvironment, IOptions<HtmlRendererOptions> htmlRendererOptions)
		{
			Engine = new RazorLightEngineBuilder().UseFileSystemProject(hostEnvironment.ContentRootPath).Build();
			Preload = htmlRendererOptions?.Value?.PreloadRazor ?? false;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			if (Preload)
				await Engine.CompileRenderStringAsync("preload_key", "<body></body>", default(object));
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public async Task<string> CompileRenderStringAsync<T>(string key, string content, T model, ExpandoObject viewBag = null)
		{
			return await Engine.CompileRenderStringAsync(key, content, model, viewBag);
		}
	}
}
