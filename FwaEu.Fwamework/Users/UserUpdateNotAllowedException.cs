using System;

namespace FwaEu.Fwamework.Users
{
	public class UserUpdateNotAllowedException : Exception
	{
		public UserUpdateNotAllowedException(string message) : base(message)
		{
		}
	}
}
