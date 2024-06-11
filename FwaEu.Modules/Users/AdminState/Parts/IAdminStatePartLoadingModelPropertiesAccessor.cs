using FwaEu.Fwamework.Users;
using System;


namespace FwaEu.Modules.Users.AdminState.Parts
{
	public interface IAdminStatePartLoadingModelPropertiesAccessor 
	{
		bool IsAdmin { get; }
		UserState State { get;  }
	}
}
