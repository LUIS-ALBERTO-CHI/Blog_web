using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public interface IHtmlRazorLayoutPathResolver
	{
		/// <summary>
		/// Get path of layout file.
		/// </summary>
		/// <returns>Path to the layout file. Returns null if you don't need to use a layout.</returns>
		string GetLayoutPath(object model);
	}
}
