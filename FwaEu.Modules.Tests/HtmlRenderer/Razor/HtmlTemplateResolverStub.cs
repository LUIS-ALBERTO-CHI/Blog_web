using FwaEu.Modules.HtmlRenderer.Razor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.HtmlRenderer.Razor
{
	public class HtmlTemplateResolverStub<TModel> : IRazorTemplateResolver<TModel>
	{
		private readonly string _template;

		public HtmlTemplateResolverStub(string template)
		{
			_template = template;
		}

		public Task<string> GetTemplateAsync()
		{
			return Task.FromResult(_template);
		}
	}
}
