using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer
{
	public interface IHtmlRenderer<TModel>
	{
		Task<string> RenderAsync(TModel model, IDictionary<string, object> parameters);
	}
}
