using System;

namespace FwaEu.Modules.PasswordRecovery
{
	public class RequestPasswordRecoveryModel
	{
		public RequestPasswordRecoveryModel(int userId, Guid guid, string newPassword)
		{
			UserId = userId;
			Guid = guid;
			NewPassword = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
		}

		public int UserId { get; }
		public Guid Guid { get; }
		public string NewPassword { get; }
	}
}
