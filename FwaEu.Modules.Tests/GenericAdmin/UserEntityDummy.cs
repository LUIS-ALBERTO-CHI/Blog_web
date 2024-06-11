using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	class UserEntityDummy : UserEntity
	{
		public new int Id => throw new NotImplementedException();
		public string FirstName => throw new NotImplementedException();
		public string LastName => throw new NotImplementedException();
		public string Email => throw new NotImplementedException();
		public new bool IsAdmin => throw new NotImplementedException();
		public new UserState State => throw new NotImplementedException();

		public new bool IsNew()
		{
			throw new NotImplementedException();
		}
	}
}
