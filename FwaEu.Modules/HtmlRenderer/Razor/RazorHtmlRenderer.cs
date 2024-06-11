using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public class RazorHtmlRenderer<TModel> : IHtmlRenderer<TModel>
	{
		private readonly IRazorTemplateResolver<TModel> _razorTemplateResolver;
		private readonly IHtmlRazorLayoutPathResolver _htmlMailRazorLayoutPathResolver;
		private readonly IMemoryCache _memoryCache;
		private readonly IRazorEngine _razorRenderer;
		public static readonly string LayoutPathKey = "LayoutPath";

		public RazorHtmlRenderer(
			IRazorTemplateResolver<TModel> razorTemplateResolver,
			IHtmlRazorLayoutPathResolver htmlMailRazorLayoutPathResolver,
			IMemoryCache memoryCache,
			IRazorEngine razorRenderer)
		{
			_razorTemplateResolver = razorTemplateResolver ?? throw new ArgumentNullException(nameof(razorTemplateResolver));
			_htmlMailRazorLayoutPathResolver = htmlMailRazorLayoutPathResolver;
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_razorRenderer = razorRenderer ?? throw new ArgumentNullException(nameof(razorRenderer));
		}

		protected ExpandoObject ConvertDictionaryToExpandoObject(IDictionary<string, object> dictionary)
		{
			var expandoObject = new ExpandoObject();

			if (dictionary != null)
			{
				foreach (var keyValuePair in dictionary)
				{
					expandoObject.TryAdd(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return expandoObject;
		}

		public async Task<string> RenderAsync(TModel model, IDictionary<string, object> parameters)
		{
			var expandoObject = ConvertDictionaryToExpandoObject(parameters);

			if (_htmlMailRazorLayoutPathResolver != null)
			{
				expandoObject.TryAdd(LayoutPathKey, _htmlMailRazorLayoutPathResolver.GetLayoutPath(model));
			}

			var cacheKey = typeof(TModel).FullName;
			var template = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
			{
				cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
				return await _razorTemplateResolver.GetTemplateAsync();
			});

			return await _razorRenderer.CompileRenderStringAsync(cacheKey, template, model, expandoObject);
		}
	}
}
