using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users
{
	interface IApplicationPartEntityPropertiesAccessor
	{
		int Id { get; }
		string FirstName { set; }
		string LastName { set; }
		string Email { get; set; }
	}
}
