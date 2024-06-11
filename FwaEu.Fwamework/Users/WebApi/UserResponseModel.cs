using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.WebApi
{
	public class UserGetResponseModel
	{
		public int Id { get; set; }
		public Dictionary<string, object> Parts { get; set; }
	}
	public class UserGetAllResponseModel
	{
		public int Id { get; set; }
		public Dictionary<string, object> Parts { get; set; }
	}

	public class UserSaveResponseModel
	{
		public int UserId { get; set; }
	}
}
