using FwaEu.Fwamework.Application;
using FwaEu.Fwamework.Temporal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Application
{
	[TestClass]
	public class DefaultApplicationInfoTests
	{
		[TestMethod]
		public void CopyrightYearsWithMatchingYears_WithReplaceableParameter()
		{
			var options = new CopyrightYearsOptionsApplicationInfoOptionsStub("2020-{0}");
			var currentDateTime = new FixedDateTime202066666Stub();
			var service = new DefaultApplicationInfo<DefaultApplicationInfoTests>(options, currentDateTime);

			Assert.AreEqual(String.Format(options.Value.CopyrightYears, currentDateTime.Now.Year),
				service.CopyrightYears);
		}

		[TestMethod]
		public void CopyrightYearsWithMatchingYears_WithoutReplaceableParameter()
		{
			const string CopyrightYears = "2020-2023";
			var options = new CopyrightYearsOptionsApplicationInfoOptionsStub(CopyrightYears);
			var currentDateTime = new FixedDateTime202066666Stub();
			var service = new DefaultApplicationInfo<DefaultApplicationInfoTests>(options, currentDateTime);

			Assert.AreEqual(CopyrightYears, service.CopyrightYears);
		}
	}
}
