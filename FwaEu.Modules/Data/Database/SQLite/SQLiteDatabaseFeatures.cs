using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database.SQLite
{
	public class SQLiteDatabaseFeatures : DatabaseFeaturesBase
	{
		// NOTE: No limit found for the column name, except the max length of statement to create it! https://www.sqlite.org/limits.html
		public override int IdentifierMaxLength => 255;
		public override string GetAllTablesSql => "SELECT name FROM sqlite_master WHERE type='table';";
		public override string[] ConnectionStringPasswordKeys => new string[] { "password" };
	}
}
