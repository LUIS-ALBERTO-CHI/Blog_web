using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Tests.Temporal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Authentication
{
	[TestClass]
	public class DateTimeClaimValueConvertServiceTests
	{
		[TestMethod]
		public void ToClaimValue()
		{
			var parisTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris");
			using (new LocalTimeZoneInfoMocker(parisTimeZone))
			{
				var service = new DateTimeClaimValueConvertService();
				var date = DateTime.Now;
				var convertedDate = service.ToClaimValue(date);
				var nowInString = new DateTimeOffset(date).ToUnixTimeSeconds().ToString();

				Assert.AreEqual(nowInString, convertedDate);
			}
		}

		[TestMethod]
		public void FromClaimValue()
		{
			var parisTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris");
			using (new LocalTimeZoneInfoMocker(parisTimeZone))
			{
				var service = new DateTimeClaimValueConvertService();
				var claimValue = "1577833200";
				var convertedClaim = service.FromClaimValue(claimValue);
				var date = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local);

				Assert.AreEqual(date, convertedClaim);
			}
		}
	}
}
