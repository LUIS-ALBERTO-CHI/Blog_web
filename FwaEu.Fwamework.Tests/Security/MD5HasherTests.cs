using FwaEu.Fwamework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Security
{
	[TestClass]
	public class MD5HasherTests
	{
		[TestMethod]
		[DataRow("thomas.rosel@fwa.eu", "af23151e5e4faa2485f40f2f30d54b1a")]
		[DataRow("J'aime le poulet grillé", "56da3f324cf6492287d6537804030db4")]
		[DataRow("€€€€€$$$!!!;?", "cd403308fac12fa8e4a889b9f105ab0d")]
		[DataRow("\u8365", "03ac65353b60123d7e00962fc5c1a24c")]
		[DataRow("荥", "03ac65353b60123d7e00962fc5c1a24c")]
		public void ToMD5String(string value, string md5ExpectedValue)
		{
			var md5Value = MD5Hasher.ToMD5String(value);
			Assert.AreEqual(md5ExpectedValue, md5Value);
		}
	}
}
