using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.ProcessResults
{
	public class ProcessResultEntryFake : ProcessResultEntry
	{
		public ProcessResultEntryFake()
			: base("Fake", "Fake content")
		{
		}
	}
}
