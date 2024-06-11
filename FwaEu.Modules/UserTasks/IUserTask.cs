using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public interface IUserTask
	{
		/// <summary>
		/// Used to deserialize from web api to model expected by UserTask
		/// </summary>
		Type ParametersType { get; }

		Task<object> ExecuteAsync(object parameters, CancellationToken cancellationToken);
		Task<TResult> ExecuteAsync<TParameters, TResult>(TParameters parameters, CancellationToken cancellationToken);
	}
}
