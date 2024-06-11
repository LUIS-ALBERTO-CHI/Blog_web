using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Permissions
{
	[TestClass]
	public class PermissionProviderAccessorTests
	{
		[TestMethod]
		public void Provider()
		{
			var services = new ServiceCollection();
			services.AddSingleton<IPermissionProviderRegister, PermissionProviderRegister>();
			services.AddTransient(typeof(IPermissionProviderAccessor<>), typeof(PermissionProviderAccessor<>));
			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<PermissionProviderStub>>();

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var accessor = serviceProvider.GetService<IPermissionProviderAccessor<PermissionProviderStub>>();
				Assert.IsNotNull(accessor);
				Assert.IsNotNull(accessor.Provider);
				Assert.IsInstanceOfType(accessor.Provider, typeof(PermissionProviderStub));
			}
		}
	}
}
