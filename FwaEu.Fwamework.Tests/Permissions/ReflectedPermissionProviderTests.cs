using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Fwamework.Tests.Permissions
{
	[TestClass]
	public class ReflectedPermissionProviderTests
	{
		[TestMethod]
		public void GetPermissions()
		{
			var provider = new PermissionProviderStub();

			var expectedPermissionInvariantIds = new[]
			{
					nameof(provider.CanAccessToReportingFake),
					nameof(provider.CanDeleteUsersFake),
			};

			var permissions = provider.GetPermissions().ToArray();
			Assert.AreEqual(expectedPermissionInvariantIds.Length, permissions.Length);

			var matches = permissions.Join(expectedPermissionInvariantIds,
				p => p.InvariantId, epii => epii, (p, epii) => epii)
				.ToArray();

			Assert.AreEqual(expectedPermissionInvariantIds.Length, matches.Length);

			Assert.IsNotNull(provider.CanAccessToReportingFake, "Must be set during the loading of the provider.");
			Assert.IsNotNull(provider.CanDeleteUsersFake, "Must be set during the loading of the provider.");
		}
	}
}
