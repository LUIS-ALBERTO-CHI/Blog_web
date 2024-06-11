using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication.JsonWebToken
{
	public class JsonWebTokenOptions
	{
		public string SigningKey { get; set; }
		public int ExpirationDelayInDays { get; set; }
		public string MetadataAddress { get; set; }
	}
}
