using FwaEu.Fwamework.ProcessResults;
using System;

namespace FwaEu.Modules.Tests.ProcessResults
{
	public class ThrowExceptionProcessResultListenerMock : IProcessResultListener
	{
		public void OnResultContextAdded(ProcessResultContext context)
		{
		}

		public void OnResultEntryAdded(ProcessResultEntry entry, ProcessResultContext context)
		{
			if (entry is ErrorProcessResultEntry error)
			{
				throw new ProcessResultEntryErrorException(
$@"Error inserted in ProcessResult.
- Content: {error.Content}
- Details: {(error.Details == null ? "null" : String.Join(",", error.Details))}");
			}
		}
	}
}
