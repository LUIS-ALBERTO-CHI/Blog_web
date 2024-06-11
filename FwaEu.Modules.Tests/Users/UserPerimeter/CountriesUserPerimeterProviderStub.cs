using FwaEu.Fwamework;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Users.UserPerimeter
{
	public class CountriesUserPerimeterProviderStubFactory : IUserPerimeterProviderFactory
	{
		public string Key => "Countries";

		public IUserPerimeterProvider Create(IServiceProvider serviceProvider)
		{
			return new CountriesUserPerimeterProviderStub();
		}
	}

	public class CountryId
	{
		public string Code { get; set; }
		public string Continent { get; set; }
	}

	public class CountriesUserPerimeterProviderStub : IUserPerimeterProvider
	{
		public Task<object[]> GetAccessibleIdsAsync(int userId)
		{
			return Task.FromResult(new object[]
			{
				new CountryId { Code = "FR", Continent = "Europe", },
				new CountryId { Code = "DE", Continent = "Europe", },
				new CountryId { Code = "CN", Continent = "Asia", },
			});
		}

		public Task<bool> HasFullAccessAsync(int userId)
		{
			return Task.FromResult(false);
		}

		public Task UpdatePerimeterAsync(int userId, bool hasFullAccess, object[] accessibleIds)
		{
			throw new NotImplementedException();
		}
	}
}
