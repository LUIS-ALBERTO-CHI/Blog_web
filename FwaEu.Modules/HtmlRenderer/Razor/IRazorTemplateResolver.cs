using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public interface IRazorTemplateResolver<TModel>
	{
		Task<string> GetTemplateAsync();
	}
}
