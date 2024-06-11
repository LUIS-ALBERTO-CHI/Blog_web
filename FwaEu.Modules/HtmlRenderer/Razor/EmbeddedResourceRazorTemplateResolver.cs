using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public class EmbeddedResourceRazorTemplateResolver<TModel> : IRazorTemplateResolver<TModel>
		where TModel : class
	{
		private readonly string _templatePath;

		public EmbeddedResourceRazorTemplateResolver()
		{
		}

		public EmbeddedResourceRazorTemplateResolver(string templatePath)
		{
			_templatePath = templatePath ?? throw new ArgumentNullException(nameof(templatePath));
		}

		public async Task<string> GetTemplateAsync()
		{
			var assembly = typeof(TModel).Assembly;
			var templatePath = _templatePath;

			if (templatePath == null)
			{
				templatePath = assembly.GetManifestResourceNames().First(rn => rn == $"{typeof(TModel).FullName}.cshtml");
			}

			using (var stream = assembly.GetManifestResourceStream(templatePath))
			using (var reader = new StreamReader(stream))
			{
				return await reader.ReadToEndAsync();
			}
		}
	}
}
