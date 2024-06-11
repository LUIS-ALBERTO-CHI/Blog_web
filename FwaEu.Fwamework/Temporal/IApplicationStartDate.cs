using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Temporal
{
	public interface IApplicationStartDate
	{
		DateTime StartedOn { get; }
	}

	public class DefaultApplicationStartDate : IApplicationStartDate
	{
		public DateTime StartedOn { get; } = DateTime.Now;
	}
}
