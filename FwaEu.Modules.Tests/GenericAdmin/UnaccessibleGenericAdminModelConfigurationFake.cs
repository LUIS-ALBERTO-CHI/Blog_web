using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	public class UnaccessibleGenericAdminModelConfigurationFake : IGenericAdminModelConfiguration
	{
		public const string Key = "UnaccessibleTest";
		public Type ModelType => null;

		public Task<DeleteResult> DeleteAsync(IEnumerable<IDictionary<string, object>> modelsKeys)
		{
			throw new NotImplementedException();
		}

		public IAuthorizedActions GetAuthorizedActions()
		{
			throw new NotImplementedException();
		}

		public Task<LoadDataResult> GetModelsAsync(GenericAdminGetModelsParameters parameters)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Property> GetProperties()
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsAccessibleAsync()
		{
			return Task.FromResult(false);
		}

		public Task<SaveResult> SaveAsync(IEnumerable<object> models)
		{
			throw new NotImplementedException();
		}
	}
}
