using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System.Text;
using System.Globalization;
using FluentNHibernate.Mapping;
using Microsoft.Extensions.Configuration;
using FwaEu.Fwamework.Text;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	// http://wiki.fluentnhibernate.org/Available_conventions#ManyToManyTableNameConvention
	public class FwaManyToManyTableNameConvention : ManyToManyTableNameConvention
	{
		public FwaManyToManyTableNameConvention(FwaConventions fwaConventions)
		{
			this._fwaConventions = fwaConventions ?? throw new ArgumentNullException(nameof(fwaConventions));
		}

		private readonly FwaConventions _fwaConventions;

		protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
		{
			return this._fwaConventions.GetLinkTableName(collection.EntityType, otherSide.EntityType); // TODO: Add member
		}

		protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
		{
			return this._fwaConventions.GetLinkTableName(collection.EntityType, collection.ChildType); // TODO: Add member
		}
	}

	// https://github.com/jagregory/fluent-nhibernate/wiki/Available-conventions
	public class FwaConventions :
		IClassConvention,
		IPropertyConvention,
		IIdConvention,
		IReferenceConvention,
		ISubclassConvention,
		ICollectionConvention,
		IJoinedSubclassConvention,
		IHasManyConvention,
		IHasManyToManyConvention
	{
		private const string ManyTableSeparator = "__";

		public class FwaConventionsOptions
		{
			public FwaConventionsOptions(IDatabaseFeatures databaseFeatures)
			{
				if (databaseFeatures == null)
				{
					throw new ArgumentNullException(nameof(databaseFeatures));
				}

				this.IdentifierMaxLength = databaseFeatures.IdentifierMaxLength;
			}

			public int IdentifierMaxLength { get; }
		}

		public FwaConventions(FwaConventionsOptions settings, IPluralizationService pluralizationService)
		{
			this._settings = settings
				?? throw new ArgumentNullException(nameof(settings));

			this._pluralizationService = pluralizationService
				?? throw new ArgumentNullException(nameof(pluralizationService));
		}

		private readonly IPluralizationService _pluralizationService;
		private readonly FwaConventionsOptions _settings;

		public static string NormalizeEntityName(Type type)
		{
			var name = type.Name;

			// The following code look like STUPID,
			// but it's not : Entity can be a part of the name of an Entity (like BudgetedEntity)
			if (name.Contains("EntityEntity"))
			{
				name = name.Replace("EntityEntity", "Entity");
			}
			else if (name.Contains("Entity"))
			{
				name = name.Replace("Entity", "");
			}

			return name;
		}

		public string GetTableNameFromType(Type type, string appendPrefix = null)
		{
			var name = NormalizeEntityName(type);
			return this.CleanUp(appendPrefix + (ToLowerSnakeCase((this._pluralizationService.Pluralize(name)))));
		}

		// see https://en.wikipedia.org/wiki/Letter_case (snake case)
		private static string ToLowerSnakeCase(string name)
		{
			var prevChar = '\'';
			var nextChar = '\'';
			var lowerSnakeCase = new StringBuilder();
			var charStr = name.ToCharArray();
			var count = name.Length;

			for (var i = 0; i <= count - 1; i++)
			{
				var currentChar = charStr[i];

				if (i > 0)
				{
					prevChar = charStr[i - 1];
				}

				if (i < count - 1)
				{
					nextChar = charStr[i + 1];
				}

				if ((i > 0) && char.IsUpper(currentChar) && (char.Equals(currentChar, '\'') || (char.IsLower(nextChar) || char.IsLower(prevChar))))
				{
					lowerSnakeCase.Append("_" + currentChar);
				}
				else
				{
					lowerSnakeCase.Append(currentChar);
				}
			}

			return lowerSnakeCase.ToString().ToLower(CultureInfo.InvariantCulture);
		}

		private string GetPropertyColumnName(IPropertyInstance propertyInstance)
			=> propertyInstance.Columns.Any() ? this.GetPropertyColumnName(propertyInstance.Columns.First().Name) : null;

		private string GetPropertyColumnName(IManyToOneInstance manyToOneInstance)
			=> this.GetPropertyColumnName(manyToOneInstance.Columns.First().Name);

		private string GetPropertyColumnName(ICollectionInstance collectionInstance)
			=> this.GetPropertyColumnName(collectionInstance.Key.Columns.First().Name);

		public string GetPropertyColumnName(string propertyName)
		{
			return this.CleanUp(ToLowerSnakeCase(propertyName));
		}

		public string GetIdColumnName(Type entityType)
		{
			return this.GetIdColumnName(ToLowerSnakeCase(NormalizeEntityName(entityType)));
		}

		private string GetIdColumnName(string value)
		{
			return this.CleanUp(value + "_id");
		}

		/// <summary>
		/// Set table name for the class mapping.
		/// </summary>
		/// <param name="instance">IClassInstance</param>
		public void Apply(IClassInstance instance)
		{
			instance.Table(this.GetTableNameFromType(instance.EntityType));
		}

		private static Type MatchWithGenericTypeDefinition(Type type, Type genericTypeDefinition)
		{
			while (type.BaseType != null)
			{
				type = type.BaseType;

				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition)
				{
					return type;
				}
			}
			return null;
		}

		private static bool IsMappedAsSubclass(Type entityType)
		{
			return entityType.Assembly.GetTypes().Any(type =>
			{
				var subclassMapType = MatchWithGenericTypeDefinition(type, typeof(SubclassMap<>));
				return subclassMapType != null && subclassMapType.GetGenericArguments()[0] == entityType;
			});
		}

		/// <summary>
		/// Set column name for the class mapping for the usual property.
		/// </summary>
		/// <param name="instance">IPropertyInstance</param>
		public void Apply(IPropertyInstance instance)
		{
			var propertyType = instance.Property.PropertyType;

			if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null
				&& !IsMappedAsSubclass(instance.Property.DeclaringType))
			{
				instance.Not.Nullable();
			}
			//NOTE: ColumnName could be empty if the we are using a Formula
			var columnName = this.GetPropertyColumnName(instance);
			if (!string.IsNullOrEmpty(columnName))
			{
				instance.Column(columnName);
				instance.CustomType(instance.Property.PropertyType);
			}
		}

		/// <summary>
		/// Set the ID column name.
		/// </summary>
		/// <param name="instance">IIdentityInstance</param>
		public void Apply(IIdentityInstance instance)
		{
			instance.Column(this.GetIdColumnName(instance.EntityType));
		}

		/// <summary>
		/// Set column name and foreign key constraint name for the many-to-one property.
		/// </summary>
		/// <param name="instance">IManyToOneInstance</param>
		public void Apply(IManyToOneInstance instance)
		{
			var columnName = this.CleanUp(this.GetPropertyColumnName(instance));
			var fk = this.CleanUp(String.Format("fk_{0}_{1}",
			this.GetTableNameFromType(instance.EntityType), columnName));

			instance.ForeignKey(fk);
			instance.Column(columnName);
		}

		public void Apply(IOneToManyCollectionInstance instance)
		{
			string linkTableName = GetLinkTableName(instance.EntityType, instance.ChildType);

			string parentColumnName = this.GetIdColumnName(instance.EntityType);
			string parentForeignKeyName = CleanUp(String.Format("fk_{0}_{1}", linkTableName, parentColumnName));
			instance.Key.Column(parentColumnName);
			instance.Key.ForeignKey(parentForeignKeyName);
		}

		/// <summary>
		/// Set discriminator value for the subclass.
		/// </summary>
		/// <param name="instance">ISubclassInstance</param>
		public void Apply(ISubclassInstance instance)
		{
			if (instance.EntityType.IsAbstract)
				instance.Abstract();
			instance.DiscriminatorValue(CleanUp(instance.EntityType.FullName));
		}

		public void Apply(ICollectionInstance instance)
		{
			instance.LazyLoad();
			var columnName = this.GetPropertyColumnName(instance);
			var fk = CleanUp(String.Format("fk_{0}_{1}", this.GetTableNameFromType(instance.EntityType), columnName));
			instance.Key.ForeignKey(fk);
		}

		/// <summary>
		/// Set many-to-many column name and foreign key constraint name for both sides.
		/// </summary>
		/// <param name="instance">IManyToManyCollectionInstance</param>
		public void Apply(IManyToManyCollectionInstance instance)
		{
			string linkTableName = GetLinkTableName(instance.EntityType, instance.ChildType);

			string parentColumnName = this.GetIdColumnName(instance.EntityType);
			string parentForeignKeyName = CleanUp(String.Format("fk_{0}_{1}", linkTableName, parentColumnName)); instance.Key.Column(parentColumnName);
			instance.Key.ForeignKey(parentForeignKeyName);

			string childColumnName = this.GetIdColumnName(ToLowerSnakeCase(NormalizeEntityName(instance.ChildType) + instance.Member.Name));
			string childForeignKeyName = CleanUp(String.Format("fk_{0}_{1}", linkTableName, childColumnName)); instance.Relationship.Column(childColumnName);
			instance.Relationship.ForeignKey(childForeignKeyName);
		}

		public string GetLinkTableName(Type parentType, Type childType)
		{
			return this.CleanUp(
				this.GetTableNameFromType(parentType)
				+ ManyTableSeparator
				+ this.GetTableNameFromType(childType));

		}

		protected string CleanUp(string value)
		{
			var identifierMaxLength = this._settings.IdentifierMaxLength;

			if (value.Length > identifierMaxLength)
			{
				var sb = new StringBuilder();

				var entireWord = true;

				foreach (var part in value.Split(new[] { '_' }, StringSplitOptions.None))
				{
					if ("fk" == part)
					{
						sb.Append("fk_");
					}
					else if (entireWord)
					{
						sb.Append(part);
						entireWord = false;
					}
					else if (String.IsNullOrEmpty(part))
					{
						sb.Append("__");
						entireWord = true;
					}
					else
					{
						sb.Append("_");
						if (part.Length >= 2)
						{
							sb.Append(part.Substring(0, 2));
						}
						else if (part.Length == 1)
						{
							sb.Append(part);
						}
					}
				}

				var s = sb.ToString();
				if (s.Length > identifierMaxLength)
				{
					return s.Substring(0, identifierMaxLength);
				}
				return s;
			}
			return value;
		}

		public void Apply(IJoinedSubclassInstance instance)
		{
			instance.Key.ForeignKey(String.Format("fk_{0}_{1}", instance.EntityType.Name, instance.Type.Name));
			instance.Key.Column(this.GetIdColumnName(instance.EntityType));

			var parentType = this.GetTableNameFromType(instance.EntityType.BaseType);
			var currentType = this.GetTableNameFromType(instance.EntityType);
			instance.Table(parentType + "_" + currentType);
		}
	}
}