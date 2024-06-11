using FwaEu.Fwamework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests
{
	[TestClass]
	public class ServiceStoreTests
	{
		[TestMethod]
		public void Dispose_Singleton()
		{
			var serviceSpy = new ServiceSpy();
			using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Singleton))
			{
				serviceStore.Add(serviceSpy);
				Assert.IsFalse(serviceSpy.IsDisposed);
			}
			Assert.IsFalse(serviceSpy.IsDisposed);
		}

		[TestMethod]
		public void Dispose_Scoped()
		{
			var serviceSpy = new ServiceSpy();
			using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped))
			{
				serviceStore.Add(serviceSpy);
				Assert.IsFalse(serviceSpy.IsDisposed);
			}
			Assert.IsTrue(serviceSpy.IsDisposed);
		}
	}

	public class ServiceSpy : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			this.IsDisposed = true;
		}
	}
}
