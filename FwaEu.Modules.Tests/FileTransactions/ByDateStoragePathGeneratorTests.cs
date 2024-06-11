using FwaEu.Modules.FileTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.FileTransactions
{
	[TestClass]
	public class ByDateStoragePathGeneratorTests
	{
		[TestMethod]
		public void GetStorageRelativePath()
		{
			var currentDateTime = new Fixed20171028CurrentDateTimeStub();
			var generator = new ByDateStoragePathGenerator(currentDateTime);

			var now = currentDateTime.Now;
			var storageRelativePath = generator.GetStorageRelativePath("toto.png");
			var expectedStart = String.Format(ByDateStoragePathGenerator.DirectoryMask, now.Year, now.Month);

			Assert.IsTrue(storageRelativePath.StartsWith(expectedStart));
		}
	}
}
