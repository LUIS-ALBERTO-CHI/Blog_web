using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	public class MockGenericAdminModelConfiguration : IGenericAdminModelConfiguration
	{
		public const string Key = "Mock";
		public Type ModelType => null;

		public Task<LoadDataResult> GetModelsAsync(GenericAdminGetModelsParameters parameters)
		{
			//NOTE: For unit tests, we don't need models but loading of ConfigurationModel will fail if exception is raised
			return Task.FromResult(
				new LoadDataResult(new ListDataSource(new List<object>(), null))
				);
		}

		public IEnumerable<Property> GetProperties()
		{
			yield return new Property("Id", typeof(int))
			{
				IsKey = true,
			};

			var nameProperty = new Property("Name", typeof(string));

			nameProperty.ExtendedProperties.Add("IsRequired", true);
			nameProperty.ExtendedProperties.Add("MaxLength", 100);
			yield return nameProperty;

			var cityIdProperty = new Property("CityId", typeof(int));

			cityIdProperty.ExtendedProperties.Add("CustomProperty", "Blabla");
			yield return cityIdProperty;
		}

		public Task<SaveResult> SaveAsync(IEnumerable<object> models)
		{
			throw new NotImplementedException();
		}

		public Task<DeleteResult> DeleteAsync(IEnumerable<IDictionary<string, object>> modelsKeys)
		{
			throw new NotImplementedException();
		}

		public IAuthorizedActions GetAuthorizedActions()
		{
			return new AuthorizedActions()
			{
				AllowCreate = true,
				AllowUpdate = true,
				AllowDelete = true,
			};
		}

		public Task<bool> IsAccessibleAsync()
		{
			return Task.FromResult(true);
		}
	}
}
