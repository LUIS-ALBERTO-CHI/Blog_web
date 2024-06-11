using FwaEu.Modules.HtmlRenderer;
using FwaEu.Modules.HtmlRenderer.Razor;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.HtmlRenderer.Razor
{
	public class UserModel
	{
		public string Name { get; set; }
	}

	[TestClass]
	public class RazorHtmlRendererTests
	{
		[TestMethod]
		public async Task RenderAsync()
		{
			const string Html =
@"<html>
    Your awesome template goes here, {0}
</html>";

			var razorTemplate =
$@"@model {typeof(UserModel).FullName}
" + String.Format(Html, "@Model.Name");

			var resolverStub = new HtmlTemplateResolverStub<UserModel>(razorTemplate);
			var razorHtmlRenderer = new RazorHtmlRenderer<UserModel>(
				resolverStub,
				null,
				new MemoryCache(new MemoryCacheOptions()),
				new DefaultRazorEngine(new FakeHostEnvironment(), Options.Create(new HtmlRendererOptions { PreloadRazor = false })));

			var userName = "Rom1 Rom1";

			Assert.AreEqual(Html.Replace("{0}", userName),
				await razorHtmlRenderer.RenderAsync(new UserModel { Name = userName }, null));
		}
	}
}
