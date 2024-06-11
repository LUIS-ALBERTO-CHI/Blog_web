using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IParametersService
	{
		Task<ParametersModel> LoadParametersAsync();
	}

	public class ParametersModel
	{
		public ParametersModel(ReadOnlyDictionary<string, object> parameters)
		{
			this.Parameters = parameters
				?? throw new ArgumentNullException(nameof(parameters));
		}

		public ReadOnlyDictionary<string, object> Parameters { get; }
	}

	public class DefaultParametersService : IParametersService
	{
		private readonly IServiceProvider _serviceProvider;
		public ParametersModel Current { get; private set; }

		public DefaultParametersService(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public async Task<ParametersModel> LoadParametersAsync()
		{
			if (this.Current == null)
			{
				var tasks = this._serviceProvider.GetServices<IParametersProvider>()
					.Select(provider => provider.LoadAsync())
					.ToArray();

				await Task.WhenAll(tasks);

				var data = tasks
					.SelectMany(task => task.Result)
					.ToDictionary(kv => kv.Key, kv => kv.Value);

				this.Current = new ParametersModel(new ReadOnlyDictionary<string, object>(data));
			}

			return this.Current;
		}
	}
}
