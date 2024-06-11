using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database.MsSql
{
	public class MsSqlDatabaseFeatures : DatabaseFeaturesBase
	{
		public override int IdentifierMaxLength => 128;
		public override string GetAllTablesSql => "SELECT name FROM sys.objects WHERE [type] = 'U';";
		public override string[] ConnectionStringPasswordKeys => new string[] { "password", "pwd" };
	}
}
