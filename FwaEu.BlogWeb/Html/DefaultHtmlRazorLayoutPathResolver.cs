using FwaEu.Modules.HtmlRenderer.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Html
{
	public class DefaultHtmlRazorLayoutPathResolver : IHtmlRazorLayoutPathResolver
	{
		private readonly string _layoutPath;

		public DefaultHtmlRazorLayoutPathResolver(string layoutPath)
		{
			_layoutPath = layoutPath ?? throw new ArgumentNullException(nameof(layoutPath));
		}

		public string GetLayoutPath(object model)
		{
			return _layoutPath;
		}
	}
}
