using FwaEu.Fwamework;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Users.UserPerimeter
{
	public class ProfilesUserPerimeterProviderStubFactory : IUserPerimeterProviderFactory
	{
		public string Key => "Profiles";

		public IUserPerimeterProvider Create(IServiceProvider serviceProvider)
		{
			return new ProfilesUserPerimeterProviderStub();
		}
	}

	public class ProfilesUserPerimeterProviderStub : IUserPerimeterProvider
	{
		public Task<object[]> GetAccessibleIdsAsync(int userId)
		{
			return Task.FromResult(default(object[]));
		}

		public Task<bool> HasFullAccessAsync(int userId)
		{
			return Task.FromResult(true);
		}

		public Task UpdatePerimeterAsync(int userId, bool hasFullAccess, object[] accessibleIds)
		{
			throw new NotImplementedException();
		}
	}
}
