using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.ProcessResults;

namespace FwaEu.Fwamework.Setup
{
	public abstract class CompositeTask<TSubTask> : ISetupTask
		where TSubTask : ISubTask
	{
		public abstract string Name { get; }
		public Type ArgumentsType => null;

		protected abstract IEnumerable<ISubTask> GetSubTasks();
		protected abstract ProcessResult CreateProcessResult();

		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var result = this.CreateProcessResult();

			foreach (var task in this.GetSubTasks())
			{
				await task.ExecuteAsync(result);
			}

			return new NoDataSetupTaskResult(result);
		}
	}

	public interface ISubTask
	{
		Task ExecuteAsync(ProcessResult result);
	}
}
