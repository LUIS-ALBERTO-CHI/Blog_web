using FwaEu.Fwamework.Temporal;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Application
{
	public class FixedDateTime202066666Stub : ICurrentDateTime
	{
		public DateTime Now => new DateTime(2020, 6, 6, 6, 6, 6);
	}
}
