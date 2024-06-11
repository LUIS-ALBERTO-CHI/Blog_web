using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public interface IUserTaskFactoryProvider
	{
		Task<IUserTaskFactory> FindFactoryAsync(string name);
		IUserTaskFactory FindFactoryNoPerimeter(Type userTaskType);

		Task<string[]> GetAccessibleTaskNamesAsync();
	}

	public class DefaultUserTaskFactoryProvider : IUserTaskFactoryProvider
	{
		private class IsAccessibleTaskResult
		{
			public string Name { get; set; }
			public bool Accessible { get; set; }
		}

		private readonly IEnumerable<IUserTaskFactory> _factories;
		private readonly IServiceProvider _serviceProvider;

		public DefaultUserTaskFactoryProvider(
			IEnumerable<IUserTaskFactory> factories,
			IServiceProvider serviceProvider)
		{
			this._factories = factories
				?? throw new ArgumentNullException(nameof(factories));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private async Task<IsAccessibleTaskResult> IsAccessibleAsync(IUserTaskFactory factory)
		{
			var accessManagerType = typeof(IUserTaskAccessManager<>).MakeGenericType(factory.UserTaskType);
			var accessManager = (IUserTaskAccessManager)this._serviceProvider.GetRequiredService(accessManagerType);

			return new IsAccessibleTaskResult
			{
				Name = factory.Name,
				Accessible = await accessManager.IsAccessibleAsync(),
			};
		}

		public IUserTaskFactory FindFactoryNoPerimeter(Type userTaskType)
		{
			return this._factories.FirstOrDefault(f => f.UserTaskType == userTaskType);
		}

		public async Task<IUserTaskFactory> FindFactoryAsync(string name)
		{
			var factory = this._factories.FirstOrDefault(f => f.Name == name);
			if (factory == null)
			{
				return null;
			}

			var isAccessibleResult = await this.IsAccessibleAsync(factory);
			if (isAccessibleResult.Accessible)
			{
				return factory;
			}

			return null;
		}

		public Task<string[]> GetAccessibleTaskNamesAsync()
		{
			var tasks = this._factories.Select(this.IsAccessibleAsync);

			// NOTE:
			// Use of WhenAll.Wait() and not await WhenAll(),
			// to get the AggregateException instead the first Exception only

			Task.WhenAll(tasks).Wait();

			var result = tasks
				.Select(t => t.Result)
				.Where(result => result.Accessible)
				.Select(result => result.Name)
				.ToArray();

			return Task.FromResult(result);
		}
	}
}
