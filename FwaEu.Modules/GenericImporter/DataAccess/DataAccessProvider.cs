using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public class DataAccessProvider
	{
		public DataAccessProvider(ServiceStore serviceStore, IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this.ServiceStore = serviceStore ?? throw new ArgumentNullException(nameof(serviceStore));
		}
		protected IServiceProvider ServiceProvider { get; }
		protected ServiceStore ServiceStore { get; }

		private readonly Dictionary<Type, IDataAccess> _loadedDataAccessInstances = new Dictionary<Type, IDataAccess>();

		protected virtual IDataAccess CreateDataAccess(Type modelType)
		{
			var factoryType = typeof(IDataAccessFactory<>).MakeGenericType(modelType);
			var factory = (IDataAccessFactory)this.ServiceProvider.GetRequiredService(factoryType);

			return factory.CreateDataAccess(this.ServiceStore);
		}

		public IDataAccess GetDataAccess(Type modelType)
		{
			if (!this._loadedDataAccessInstances.ContainsKey(modelType))
			{
				this._loadedDataAccessInstances.Add(modelType, this.CreateDataAccess(modelType));
			}

			return this._loadedDataAccessInstances[modelType];
		}
	}
}
