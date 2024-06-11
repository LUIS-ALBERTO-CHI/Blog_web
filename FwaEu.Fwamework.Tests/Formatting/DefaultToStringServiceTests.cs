using FwaEu.Fwamework.Formatting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FwaEu.Fwamework.Tests.Formatting
{
	[TestClass]
	public class DefaultToStringServiceTests
	{
		[TestMethod]
		[DataRow(123.4567, null, "N2", "123.46")]
		[DataRow(123.4567, "fr", "N2", "123,46")]
		[DataRow(123.4567, null, "N5", "123.45670")]
		[DataRow(123.4567, "fr", "N5", "123,45670")]
		public void ToString_FormattableObject(
			IFormattable formattableValue, string cultureName,
			string format, string expectedValue)
		{
			var service = new DefaultToStringService();
			var culture = cultureName == null
				? CultureInfo.InvariantCulture
				: new CultureInfo(cultureName);

			var @string = service.ToString(formattableValue, format, culture);

			Assert.AreEqual(expectedValue, @string);
		}

		[TestMethod]
		[DataRow("Roberto", "iv", "xxx", "Roberto")]
		[DataRow("Roberto", "iv", null, "Roberto")]
		public void ToString_NonFormattableObject(
			object value, string cultureName,
			string format, string expectedValue)
		{
			var service = new DefaultToStringService();
			var culture = new CultureInfo(cultureName);

			var @string = service.ToString(value, format, culture);

			Assert.AreEqual(expectedValue, @string);
		}
	}
}
