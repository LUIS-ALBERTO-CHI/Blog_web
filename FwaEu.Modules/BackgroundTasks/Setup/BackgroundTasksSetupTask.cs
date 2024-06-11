using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks.Setup
{
	public class BackgroundTasksModel
	{
		public BackgroundTaskStateModel[] BackgroundTasks { get; set; }
		public BackgroundTasksModel(BackgroundTaskStateModel[] backgroundTasks)
		{
			BackgroundTasks = backgroundTasks;
		}
	}

	public class BackgroundTasksSetupTask : ISetupTask
	{
		private IEnumerable<IBackgroundTasksService> _backgroundTasksServices { get; }

		public BackgroundTasksSetupTask(IEnumerable<IBackgroundTasksService> backgroundTasksServices)
		{
			_backgroundTasksServices = backgroundTasksServices ?? throw new ArgumentNullException(nameof(backgroundTasksServices));
		}

		public string Name => "BackgroundTasks";

		public Type ArgumentsType => null;

		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var backgroundTasks = (await Task.WhenAll(_backgroundTasksServices.Select(bts => bts.GetAllStatesAsync())))
				.SelectMany(r => r);

			return new SetupTaskResult<BackgroundTasksModel>(
				new ProcessResult(), new BackgroundTasksModel(backgroundTasks.ToArray()));
		}
	}
}
