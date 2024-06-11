using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IDatabaseFeaturesProvider
	{
		Type DatabaseFeaturesType { get; }
		IDatabaseFeatures GetDatabaseFeatures();
	}

	public class DatabaseFeaturesProvider : IDatabaseFeaturesProvider
	{
		public Type DatabaseFeaturesType { get; }

		public DatabaseFeaturesProvider(Type databaseFeaturesType)
		{
			DatabaseFeaturesType = databaseFeaturesType;
		}

		public DatabaseFeaturesProvider(string databaseFeaturesFullTypeName)
			: this(Type.GetType(databaseFeaturesFullTypeName))
		{
		}

		public IDatabaseFeatures GetDatabaseFeatures()
		{
			return (IDatabaseFeatures)Activator.CreateInstance(DatabaseFeaturesType);
		}
	}
	public class DatabaseFeaturesProvider<TFeatures> : IDatabaseFeaturesProvider
		where TFeatures : IDatabaseFeatures, new()
	{
		public Type DatabaseFeaturesType => typeof(TFeatures);

		public IDatabaseFeatures GetDatabaseFeatures()
		{
			return new TFeatures();
		}
	}

	public interface IDatabaseFeatures
	{
		int IdentifierMaxLength { get; }
		string GetAllTablesSql { get; }
		string[] ConnectionStringPasswordKeys { get; }
		Exception CreateException(GenericADOException exception);
	}
}
