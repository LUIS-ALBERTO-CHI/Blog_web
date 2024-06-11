using FwaEu.Fwamework.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FwaEu.Fwamework.Tests.WebAPI
{
	[TestClass]
	public class ApplicationAllowedCheckServiceTest
	{
		[TestMethod]
		public void IsApplicationAllowed_WithFilters()
		{
			var service = new ApplicationAllowedCheckService(new OptionsAllowedApplicationsOptionsStub());

			Assert.IsFalse(service.IsApplicationAllowed("Roberto", "Carlos", "0.0.0.1"));
			Assert.IsTrue(service.IsApplicationAllowed("Roberto", "Martin", "0.0.0.1"));
			Assert.IsFalse(service.IsApplicationAllowed("Roberto", "Martin", "7.7.7.7"));
		}

		[TestMethod]
		public void IsApplicationAllowed_WithoutFilters()
		{
			var service = new ApplicationAllowedCheckService(new OptionsAllowedApplicationsOptionsNoFilterStub());

			Assert.IsTrue(service.IsApplicationAllowed("Roberto", "Martin", "7.7.7.7"));
			Assert.IsFalse(service.IsApplicationAllowed("Roberto", "Carlos", "7.7.7.7"));
		}

		[TestMethod]
		public void IsApplicationAllowed_WithEmptyFilters()
		{
			var service = new ApplicationAllowedCheckService(new OptionsAllowedApplicationsOptionsEmptyFilterStub());

			Assert.IsFalse(service.IsApplicationAllowed("Roberto", "Martin", "7.7.7.7"));
			Assert.IsFalse(service.IsApplicationAllowed("Roberto", "Carlos", "7.7.7.7"));
		}
	}
}

