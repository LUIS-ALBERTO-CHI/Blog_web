using FwaEu.Modules.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Reports
{
	[TestClass]
	public class ReportLocalizableStringTests
	{
		[TestMethod]
		public void GetString_NoFallback()
		{
			var localizableString = new ReportLocalizableString()
			{
				{"en", "My iphone is bigger than your iphone" },
				{"fr", "Mon iphone est plus grand que ton iphone" },
			};

			var value = localizableString.GetString("fr", "en");
			Assert.AreEqual("Mon iphone est plus grand que ton iphone", value);
		}

		[TestMethod]
		public void GetString_Fallback_KeyNotFound()
		{
			var localizableString = new ReportLocalizableString()
			{
				{"en", "My iphone is bigger than your iphone" },
			};

			var value = localizableString.GetString("fr", "en");
			Assert.AreEqual("My iphone is bigger than your iphone", value);
		}

		[TestMethod]
		public void GetString_Fallback_ValueNullOrEmpty()
		{
			var localizableString = new ReportLocalizableString()
			{
				{"en", "My iphone is bigger than your iphone" },
				{"fr", "" },
			};

			var value = localizableString.GetString("fr", "en");
			Assert.AreEqual("My iphone is bigger than your iphone", value);
		}

		[TestMethod]
		public void GetString_NoValue()
		{
			var localizableString = new ReportLocalizableString()
			{
			};

			var value = localizableString.GetString("fr", "en");
			Assert.IsNull(value);
		}
	}
}
