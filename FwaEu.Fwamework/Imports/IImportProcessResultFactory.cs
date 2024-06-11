using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public interface IImportProcessResultFactory
	{
		ProcessResult CreateProcessResult();
	}

	public class DefaultImportProcessResultFactory : IImportProcessResultFactory
	{
		public ProcessResult CreateProcessResult()
		{
			var processResult = new ProcessResult();
			processResult.Listener = new StopProcessAfterTooManyErrorsProcessResultListener();
			return processResult;
		}
	}
}
