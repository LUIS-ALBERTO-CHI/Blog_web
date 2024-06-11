using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface ICurrentUserPerimeterService
	{
		Task LoadFullAccessesAsync();
		string[] FullAccessKeys { get; }
	}

	public class DefaultCurrentUserPerimeterService : ICurrentUserPerimeterService
	{
		public DefaultCurrentUserPerimeterService(IUserPerimeterService userPerimeterService,
			ICurrentUserService currentUserService)
		{
			this._userPerimeterService = userPerimeterService
				?? throw new ArgumentNullException(nameof(userPerimeterService));

			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));
		}

		private readonly IUserPerimeterService _userPerimeterService;
		private readonly ICurrentUserService _currentUserService;

		public string[] FullAccessKeys { get; private set; } = new string[] { }; // NOTE: To make easier test of perimeters in DataFilters

		public async Task LoadFullAccessesAsync()
		{
			if (this._currentUserService.User != null)
			{
				var user = this._currentUserService.User.Entity;

				var fullAccesses = await this._userPerimeterService
					.GetFullAccessPerimeterKeysAsync(user.Id);

				this.FullAccessKeys = fullAccesses ?? new string[] { }; // NOTE: To make easier test of perimeters in DataFilters
			}
		}
	}
}
