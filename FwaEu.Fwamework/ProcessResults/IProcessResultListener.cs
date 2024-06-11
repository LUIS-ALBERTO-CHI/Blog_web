using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public interface IProcessResultListener
	{
		void OnResultContextAdded(ProcessResultContext context);
		void OnResultEntryAdded(ProcessResultEntry entry, ProcessResultContext context);
	}
}
