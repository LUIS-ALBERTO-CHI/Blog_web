using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public interface ISetupTaskResult
	{
		ProcessResult ProcessResult { get; }
		object Data { get; }
	}

	public class NoDataSetupTaskResult : ISetupTaskResult
	{
		public NoDataSetupTaskResult(ProcessResult processResult)
		{
			this.ProcessResult = processResult ?? throw new ArgumentNullException(nameof(processResult));
		}

		public ProcessResult ProcessResult { get; }
		object ISetupTaskResult.Data => null;
	}

	public class SetupTaskResult<TData> : ISetupTaskResult
	{
		public SetupTaskResult(ProcessResult processResult, TData data)
		{
			this.ProcessResult = processResult ?? throw new ArgumentNullException(nameof(processResult));
			this.Data = data;
		}

		public ProcessResult ProcessResult { get; }
		public TData Data { get; }

		object ISetupTaskResult.Data => this.Data;
	}
}
