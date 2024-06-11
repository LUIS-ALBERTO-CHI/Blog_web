using FwaEu.Fwamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public class DefaultUserPerimeterService : IUserPerimeterService
	{
		public DefaultUserPerimeterService(UserPerimeterProviderFactoryCache factories,
			IServiceProvider serviceProvider)
		{
			this._factories = factories
				?? throw new ArgumentNullException(nameof(factories));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly UserPerimeterProviderFactoryCache _factories;
		private readonly IServiceProvider _serviceProvider;

		private IEnumerable<(string Key, IUserPerimeterProvider Provider)> GetProviders(string key = null)
		{
			var factories = this._factories.GetAll();

			if (key != null)
			{
				factories = factories.Where(f => f.Key == key);
			}

			return factories.Select(f => (f.Key, f.Create(this._serviceProvider)));
		}

		private (string Key,
			IUserPerimeterProvider Provider,
			Task<bool> HasFullAccessTasks)[] GetFullAccessTasks(int userId)
		{
			return this.GetProviders()
				.Select(p => (p.Key, p.Provider, p.Provider.HasFullAccessAsync(userId)))
				.ToArray();
		}

		public async Task<UserPerimeterModel> GetAccessAsync(int userId, string perimeterKey)
		{
			var provider = this.GetProviders(perimeterKey).First().Provider;

			var hasFullAccess = await provider.HasFullAccessAsync(userId);
			if (hasFullAccess)
			{
				return new UserPerimeterModel(perimeterKey, true, null);
			}

			var accessibleIds = await provider.GetAccessibleIdsAsync(userId);
			return new UserPerimeterModel(perimeterKey, false, accessibleIds);
		}

		public async Task<UserPerimeterModel[]> GetAccessesAsync(int userId)
		{
			var fullAccessTasks = this.GetFullAccessTasks(userId);

			if (!fullAccessTasks.Any())
			{
				return new UserPerimeterModel[] { };
			}

			await Task.WhenAll(fullAccessTasks.Select(t => t.HasFullAccessTasks));

			var restrictedAccessTasks = fullAccessTasks
				.Where(t => !t.HasFullAccessTasks.Result)
				.Select(t => new
				{
					t.Key,
					GetAccessibleIdsTasks = t.Provider.GetAccessibleIdsAsync(userId),
				})
				.ToArray();

			var result = fullAccessTasks
				.Where(t => t.HasFullAccessTasks.Result)
				.Select(t => new UserPerimeterModel(t.Key, true, null));

			if (restrictedAccessTasks.Any())
			{
				await Task.WhenAll(restrictedAccessTasks.Select(t => t.GetAccessibleIdsTasks));

				result = result.Concat(restrictedAccessTasks
					.Select(t => new UserPerimeterModel(t.Key, false, t.GetAccessibleIdsTasks.Result)));
			}

			return result.ToArray();
		}

		public async Task<string[]> GetFullAccessPerimeterKeysAsync(int userId)
		{
			var tasks = this.GetFullAccessTasks(userId);
			await Task.WhenAll(tasks.Select(t => t.HasFullAccessTasks));

			return tasks
				.Where(t => t.HasFullAccessTasks.Result)
				.Select(t => t.Key)
				.ToArray();
		}

		public async Task UpdatePerimeterAsync(int userId, UserPerimeterModel perimeter)
		{
			var provider = this._factories.GetFactory(perimeter.Key)
				.Create(this._serviceProvider);

			await provider.UpdatePerimeterAsync(
				userId, perimeter.HasFullAccess,
				perimeter.AccessibleIds);
		}
	}
}
