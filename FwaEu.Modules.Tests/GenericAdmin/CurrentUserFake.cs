using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	//TODO: To remove after IGenericAdminUserProvider implementation https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4396
	class CurrentUserFake : ICurrentUser
	{
		public string Identity => throw new NotImplementedException();
		public UserEntity Entity => new UserEntityDummy();
	}
}
