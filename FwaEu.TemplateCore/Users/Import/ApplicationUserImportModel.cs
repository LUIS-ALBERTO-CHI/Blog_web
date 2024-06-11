using FwaEu.Fwamework.Users;
using FwaEu.Modules.GenericImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users.Import
{
	public class ApplicationUserImportModel
	{
		public ApplicationUserImportModel(ApplicationUserEntity entity, DataRow dataRow)
		{
			this.Entity = entity;
			this.DataRow = dataRow ?? throw new ArgumentNullException(nameof(dataRow));
		}

		public ApplicationUserEntity Entity { get; private set; }
		public DataRow DataRow { get; private set; }

		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Fwamework.Users.UserState? State { get; set; }
		public string Password { get; set; }
	}
}
