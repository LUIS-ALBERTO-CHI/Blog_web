using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public class CurrentUserParametersProvider : IParametersProvider
	{
		public const string CurrentUserIdKey = "CurrentUserId";
		public const string CurrentUserIdentityKey = "CurrentUserIdentity";

		private readonly ICurrentUserService _currentUserService;

		public CurrentUserParametersProvider(ICurrentUserService currentUserService)
		{
			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));
		}

		public Task<Dictionary<string, object>> LoadAsync()
		{
			var currentUser = this._currentUserService.User;
			var result = new Dictionary<string, object>()
			{
				{ CurrentUserIdKey, currentUser.Entity.Id },
				{ CurrentUserIdentityKey, currentUser.Identity },
			};

			return Task.FromResult(result);
		}
	}
}
