using FwaEu.Fwamework.ProcessResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Tests.ProcessResults
{
	[TestClass]
	public class StopProcessAfterTooManyErrorsProcessResultListenerTests
	{
		[TestMethod]
		public void TooManyErrors_MustFail()
		{
			var maximumErrorCount = 10;
			var listener = new StopProcessAfterTooManyErrorsProcessResultListener(maximumErrorCount);

			var processResult = new ProcessResult() { Listener = listener };
			var context = processResult.CreateContext("MyContext", "Test");

			Assert.ThrowsException<ProcessStoppedAfterTooManyErrorsException>(() =>
		   {
			   for (var i = 0; i < maximumErrorCount; i++)
			   {
				   context.Add(new ErrorProcessResultEntry($"Error {i}"));
			   }
		   });
		}
	}
}
