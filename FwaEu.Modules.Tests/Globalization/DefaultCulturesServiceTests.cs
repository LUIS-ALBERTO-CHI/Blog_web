using FwaEu.Fwamework.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FwaEu.Modules.Tests.Globalization
{
	[TestClass]
	public class DefaultCulturesServiceTests
	{
		[TestMethod]
		public void Constructor_MustCrashWhenDefaultIsNotInAvailableCultures()
		{
			var fr = new CultureInfo("fr-FR");
			var en = new CultureInfo("en-EN");

			Assert.ThrowsException<ArgumentException>(
				() => _ = new DefaultCulturesService(fr, new[] { en }));
		}
	}
}
