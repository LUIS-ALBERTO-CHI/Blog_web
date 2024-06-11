using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database.MySql
{
	public class MySqlDatabaseFeatures : DatabaseFeaturesBase
	{
		public override int IdentifierMaxLength => 63;
		public override string GetAllTablesSql => "SHOW TABLES;";
		public override string[] ConnectionStringPasswordKeys => new string[] { "password", "pwd" };
		protected override bool IsDeleteConstraint(DbException exception)
		{
			// NOTE: If you want handle the exception from exception.Data by using the key "Server Error Code" or the value "1451" exception.Data["Server Error Code"].ToString().Contains("1451")
			return exception.Message.ToUpper().Contains("DELETE")
				|| base.IsDeleteConstraint(exception);
		}

		protected override bool IsUniqueConstraint(DbException exception)
		{
			// NOTE: If you want handle the exception from exception.Data by using the key "Server Error Code" or the value "1062" exception.Data["Server Error Code"].ToString().Contains("1062")
			return exception.Message.ToUpper().Contains("DUPLICATE")
				|| base.IsUniqueConstraint(exception);
		}
	}
}
