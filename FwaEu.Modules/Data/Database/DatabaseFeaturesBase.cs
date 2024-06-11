using FwaEu.Fwamework.Data.Database;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database
{
	public abstract class DatabaseFeaturesBase : IDatabaseFeatures
	{
		public abstract int IdentifierMaxLength { get; }
		public abstract string GetAllTablesSql { get; }
		public abstract string[] ConnectionStringPasswordKeys { get; }

		protected virtual bool IsDeleteConstraint(System.Data.Common.DbException exception)
		{
			return exception.Message.Contains("DELETE");
		}

		protected virtual bool IsUniqueConstraint(System.Data.Common.DbException exception)
		{
			return exception.Message.Contains("UNIQUE");
		}

		public virtual Exception CreateException(GenericADOException exception)
		{
			if (exception.InnerException is System.Data.Common.DbException inner)
			{
				if (this.IsDeleteConstraint(inner))
				{
					return new DatabaseException(
						"This item can not be deleted because it is used in the application.",
						"DeleteDbConstraint", exception);
				}
				else if (this.IsUniqueConstraint(inner))
				{
					return new DatabaseException(
						"This item can not be created or updated because it does not respect a database unique constraint.",
						"UniqueDbConstraint", exception);
				}
			}

			return null;
		}
	}
}
