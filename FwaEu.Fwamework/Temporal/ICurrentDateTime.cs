using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Temporal
{
	public interface ICurrentDateTime
	{
		DateTime Now { get; }
	}

	public class DateTimeNowCurrentDateTime : ICurrentDateTime
	{
		public DateTime Now => DateTime.Now;
	}
}
