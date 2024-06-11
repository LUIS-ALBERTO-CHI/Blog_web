using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public abstract class UserTask<TParameters, TResult> : IUserTask
	{
		public Type ParametersType => typeof(TParameters);

		public abstract Task<TResult> ExecuteAsync(TParameters parameters, CancellationToken cancellationToken);

		async Task<object> IUserTask.ExecuteAsync(object parameters, CancellationToken cancellationToken)
		{
			return await this.ExecuteAsync((TParameters)parameters, cancellationToken);
		}

		async Task<T2> IUserTask.ExecuteAsync<T1, T2>(T1 parameters, CancellationToken cancellationToken)
		{
			var result = await this.ExecuteAsync((TParameters)(object)parameters, cancellationToken);
			return (T2)(object)result;
		}
	}

	public abstract class UserTask<TResult> : UserTask<Dictionary<string, object>, TResult>
	{
		public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);

		public override Task<TResult> ExecuteAsync(Dictionary<string, object> parameters, CancellationToken cancellationToken)
		{
			return this.ExecuteAsync(cancellationToken);
		}
	}
}
