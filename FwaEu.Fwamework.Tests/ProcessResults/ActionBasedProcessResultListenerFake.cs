using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace FwaEu.Fwamework.Tests.ProcessResults
{
	public class ActionBasedProcessResultListenerFake : IProcessResultListener
	{
		private readonly Action<ProcessResultContext>? _actionOnResultContextAdded;
		private readonly Action<ProcessResultEntry, ProcessResultContext>? _actionOnResultEntryAdded;

		public ActionBasedProcessResultListenerFake(
			Action<ProcessResultContext>? actionOnResultContextAdded = null,
			Action<ProcessResultEntry, ProcessResultContext>? actionOnResultEntryAdded = null)
		{
			this._actionOnResultContextAdded = actionOnResultContextAdded;
			this._actionOnResultEntryAdded = actionOnResultEntryAdded;
		}

		public void OnResultContextAdded(ProcessResultContext context)
		{
			this._actionOnResultContextAdded?.Invoke(context);
		}

		public void OnResultEntryAdded(ProcessResultEntry entry, ProcessResultContext context)
		{
			this._actionOnResultEntryAdded?.Invoke(entry, context);
		}
	}
}
#nullable disable
