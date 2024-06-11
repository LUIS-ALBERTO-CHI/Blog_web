using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class GetAllUsersTask : ISetupTask
	{
		public string Name => "GetAllUsers";
		public Type ArgumentsType => null;

		public GetAllUsersTask(IUserService userService)
		{
			this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		private readonly IUserService _userService;

		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var users = await _userService.GetAllAsync();
			var userModels = users.Select(u => new UserModel(u.Id, u.Identity, u.Parts)).ToArray();

			return new SetupTaskResult<GetAllUsersResultModel>(
						new ProcessResult(),
						new GetAllUsersResultModel(userModels));
		}
	}

	public class GetAllUsersResultModel
	{
		public GetAllUsersResultModel(UserModel[] users)
		{
			this.Users = users ?? throw new ArgumentNullException(nameof(users));
		}

		public UserModel[] Users { get; }
	}

	public class UserModel
	{
		public UserModel(int id, string identity, Dictionary<string, object> parts)
		{
			Id = id;
			Identity = identity ?? throw new ArgumentNullException(nameof(identity));
			Parts = parts ?? throw new ArgumentNullException(nameof(parts));
		}

		public int Id { get; }
		public string Identity { get; }
		public Dictionary<string, object> Parts { get; set; }
	}
}
