using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public class DateTimeClaimValueConvertService : IClaimValueConvertService<DateTime, string>
	{ 
		public DateTime FromClaimValue(string claimValue)
		{
			var timestamp = long.Parse(claimValue);
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			return epoch.AddSeconds(timestamp).ToLocalTime();
		}

		public string ToClaimValue(DateTime value)
		{
			long unixTime = ((DateTimeOffset)value).ToUnixTimeSeconds();
			return unixTime.ToString();
		}
	}
}
