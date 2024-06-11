using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	//TODO: To remove after IGenericAdminUserProvider implementation https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4396
	class FakeCurrentUserService : ICurrentUserService
	{
		public ICurrentUser User => new CurrentUserFake();

		public Task<CurrentUserLoadResult> LoadAsync()
		{
			throw new NotImplementedException();
		}
	}
}
