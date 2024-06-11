using FwaEu.Fwamework.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Globalization
{
	[TestClass]
	public class DefaultCultureResolverTests
	{
		[TestMethod]
		public void ResolveBestCulture_FindFromPreferred()
		{
			var culturesService = new FrenchEnglishCulturesServiceStub();
			var resolver = new DefaultCultureResolver(culturesService);

			var preferredCultures = new[] { "de", "ch", "fr" };
			var resolvedCulture = resolver.ResolveBestCulture(preferredCultures);

			Assert.AreEqual("fr", resolvedCulture.TwoLetterISOLanguageName);
		}

		[TestMethod]
		public void ResolveBestCulture_FindFromNull()
		{
			var culturesService = new FrenchEnglishCulturesServiceStub();
			var resolver = new DefaultCultureResolver(culturesService);

			var resolvedCulture = resolver.ResolveBestCulture(null);

			Assert.AreEqual(culturesService.DefaultCulture, resolvedCulture,
				"The resolved culture must be the default culture (with the current stub: \"fr\"))");
		}

		[TestMethod]
		public void ResolveBestCulture_FallbackToDefaultCultureWhenNoMatchWithPreffered()
		{
			var culturesService = new FrenchEnglishCulturesServiceStub();
			var resolver = new DefaultCultureResolver(culturesService);

			var preferredCultures = new[] { "de", "ch", "cn" };
			var resolvedCulture = resolver.ResolveBestCulture(preferredCultures);

			Assert.AreEqual(culturesService.DefaultCulture, resolvedCulture,
				"The resolved culture must be the default culture (with the current stub: \"fr\"))");
		}
	}
}
