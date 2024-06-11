using FwaEu.Fwamework.ProcessResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public interface ISetupTask
	{
		string Name { get; }
		Type ArgumentsType { get; }

		Task<ISetupTaskResult> ExecuteAsync(object arguments);
	}

	public abstract class SetupTask<TArguments, TResult> : ISetupTask
		where TResult : ISetupTaskResult
	{
		public abstract string Name { get; }
		public Type ArgumentsType => typeof(TArguments);

		public abstract Task<TResult> ExecuteAsync(TArguments arguments);

		async Task<ISetupTaskResult> ISetupTask.ExecuteAsync(object arguments)
		{
			return await this.ExecuteAsync((TArguments)arguments);
		}
	}

	public abstract class SetupTask<TArguments> : SetupTask<TArguments, NoDataSetupTaskResult>
	{

	}
}
