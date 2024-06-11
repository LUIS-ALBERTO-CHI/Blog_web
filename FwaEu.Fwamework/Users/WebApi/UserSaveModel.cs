using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.WebApi
{
	public class UserSaveModel
	{
		public JObject Parts { get; set; }
	}
}
