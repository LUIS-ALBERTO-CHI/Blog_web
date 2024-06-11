using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsPerimeters
{
	public class ReportReferenceEntityService : IReferenceEntityService<string, string>
	{
		public Task<string> GetAsync(string id)
		{
			return Task.FromResult(id); // NOTE: The string is id the invariant id, and we simulate the entity by the string itself
		}
	}
}
