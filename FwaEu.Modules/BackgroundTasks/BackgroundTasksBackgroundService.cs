using FwaEu.Fwamework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public class BackgroundTasksBackgroundService : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly AsyncLocalScopedServiceProviderAccessor _asyncLocalScopedServiceProviderAccessor;
		private readonly ILogger<BackgroundTasksBackgroundService> _logger;

		public BackgroundTasksBackgroundService(IServiceProvider serviceProvider,
			AsyncLocalScopedServiceProviderAccessor asyncLocalScopedServiceProviderAccessor,
			ILogger<BackgroundTasksBackgroundService> logger)
		{
			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._asyncLocalScopedServiceProviderAccessor = asyncLocalScopedServiceProviderAccessor
			?? throw new ArgumentNullException(nameof(asyncLocalScopedServiceProviderAccessor));

			this._logger = logger
			?? throw new ArgumentNullException(nameof(logger));
		}

		protected async override Task ExecuteAsync(CancellationToken cancellationToken)
		{
			var taskCancellationToken = default(CancellationTokenSource);
			var delayCancellationToken = default(CancellationTokenSource);

			cancellationToken.Register(() =>
			{
				taskCancellationToken?.Cancel();
				taskCancellationToken = null;

				delayCancellationToken?.Cancel();
				delayCancellationToken = null;
			});
			using (var scope = _serviceProvider.CreateScope())
			using (_asyncLocalScopedServiceProviderAccessor.BeginScope(scope.ServiceProvider))
			{
				var backgroundTasksService = scope.ServiceProvider.GetService<IBackgroundTasksService>();

				backgroundTasksService.OnQueueItemAdded(tsp =>
				{
					delayCancellationToken?.Cancel();
					delayCancellationToken = null;
				});

				while (!cancellationToken.IsCancellationRequested)
				{
					try
					{
						taskCancellationToken = new CancellationTokenSource();
						await backgroundTasksService.WakeUpAsync(taskCancellationToken.Token);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error during background tasks service wake up");
					}
					finally
					{
						taskCancellationToken = null;
					}

					try
					{
						delayCancellationToken = new CancellationTokenSource();

						await Task.Delay(
							backgroundTasksService.PoolingDelayInMilliseconds,
							delayCancellationToken.Token);
					}
					catch (TaskCanceledException)
					{
						//NOTE: Nothing to do
					}
					finally
					{
						delayCancellationToken = null;
					}
				}
			}
		}
	}
}
