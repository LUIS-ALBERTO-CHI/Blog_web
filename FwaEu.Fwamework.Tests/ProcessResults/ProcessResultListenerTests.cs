using FwaEu.Fwamework.ProcessResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.ProcessResults
{
	[TestClass]
	public class ProcessResultListenerTests
	{
		[TestMethod]
		public void ContextAdded()
		{
			var listenerContext = default(ProcessResultContext);

			var listener = new ActionBasedProcessResultListenerFake(
				actionOnResultContextAdded: context => listenerContext = context
				);

			var processResult = new ProcessResult() { Listener = listener };
			var createdContext = processResult.CreateContext("MyContext", "Test");

			Assert.AreEqual(listenerContext, createdContext);
		}

		[TestMethod]
		public void EntryAdded()
		{
			var listenerEntry = default(ProcessResultEntry);

			var listener = new ActionBasedProcessResultListenerFake(
				actionOnResultEntryAdded: (entry, context) => listenerEntry = entry
				);

			var fakeEntry = new ProcessResultEntryFake();

			var processResult = new ProcessResult() { Listener = listener };
			var context = processResult.CreateContext("MyContext", "Test");
			context.Add(fakeEntry);

			Assert.AreEqual(listenerEntry, fakeEntry);
		}
	}
}
