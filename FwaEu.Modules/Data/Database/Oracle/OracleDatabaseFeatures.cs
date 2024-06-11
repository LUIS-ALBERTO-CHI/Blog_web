using FwaEu.Fwamework.Data.Database;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database.Oracle
{
	public class OracleDatabaseFeatures : DatabaseFeaturesBase
	{
		public override int IdentifierMaxLength => 30;
		public override string GetAllTablesSql => "SELECT table_name FROM user_tables;";
		public override string[] ConnectionStringPasswordKeys => new string[] { "password" };

		protected override bool IsDeleteConstraint(DbException exception)
		{
			return exception.Message.Contains("ORA-02292")
				|| base.IsDeleteConstraint(exception);
		}
	}
}
