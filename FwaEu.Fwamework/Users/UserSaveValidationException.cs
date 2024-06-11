using System;

namespace FwaEu.Fwamework.Users
{
	public class UserSaveValidationException : Exception
	{
		public UserSaveValidationException(string userPart, string errorType, string message) : base(message)
		{
			this.ErrorType = errorType
				?? throw new ArgumentNullException(nameof(errorType));
			this.UserPart = userPart;	
		}

		public string ErrorType { get; }
		public string UserPart { get; }
	}
}
