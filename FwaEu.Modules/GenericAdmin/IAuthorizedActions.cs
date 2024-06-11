using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IAuthorizedActions
	{
		bool AllowCreate { get; }
		bool AllowUpdate { get; }
		bool AllowDelete { get; }
	}

	public class AuthorizedActions : IAuthorizedActions
	{
		public bool AllowCreate { get; set; }
		public bool AllowUpdate { get; set; }
		public bool AllowDelete { get; set; }
	}
}