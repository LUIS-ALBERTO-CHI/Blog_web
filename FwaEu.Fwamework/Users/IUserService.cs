using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface IUserService
	{
		Task<IEnumerable<UserListModel>> GetAllAsync();
		Task<IEnumerable<UserListModel>> GetAllForAdminAsync();
		Task<UserEditModel> GetAsync(int id);
		Task<UserEditModel> GetForAdminAsync(int id);
		Task<int> SaveAsync(UserSaveModel user);

		Dictionary<string, IPartHandler> GetSaveHandlerByPartName();
	}

	public class UserModelBase
	{
		public int Id { get; set; }
		public Dictionary<string, object> Parts { get; set; }
	}

	public class UserListModel : UserModelBase
	{
		public string Identity { get; set; }
	}

	public class UserEditModel : UserModelBase
	{
	}

	public class UserSaveModel : UserModelBase
	{
		public UserSaveModel(Dictionary<string, object> parts)
		{
			this.Parts = parts ?? throw new ArgumentNullException(nameof(parts));
		}
	}
}
