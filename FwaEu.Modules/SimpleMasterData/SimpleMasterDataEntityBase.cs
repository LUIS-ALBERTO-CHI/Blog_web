using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace FwaEu.Modules.SimpleMasterData
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<SimpleMasterDataEntityBase>))]
	public abstract class SimpleMasterDataEntityBase : ICreationAndUpdateTracked
	{
		public int Id { get; set; }
		public string InvariantId { get; set; }


		[LocalizableString]
		public IDictionary Name { get; set; }

		public DateTime UpdatedOn { get; set; }
		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}

		public override string ToString()
		{
			return this.Name.ToStringFirstValue();
		}
	}

	public class SimpleMasterDataClassMapOptions
	{
		/// <summary>
		/// Will only map the DefaultCulture, not the others.
		/// Will create only one column in the database.
		/// </summary>
		public bool MapOnlyDefaultCultureForName { get; set; } = false;

		/// <summary>
		/// Allows you to declare multiple unique keys base on 
		/// other(s) property(ies), including the InvariantId property.
		/// </summary>
		public string[] InvariantIdUniqueKeys { get; set; }

		/// <summary>
		/// Allows you to configure the Name property dynamic component.
		/// See documentation on LocalizableStringMappingConfig.
		/// </summary>
		public LocalizableStringMappingConfig NameMappingConfig { get; set; }
	}

	public abstract class SimpleMasterDataEntityBaseClassMap<TEntity> : ClassMap<TEntity>
		where TEntity : SimpleMasterDataEntityBase
	{
		protected SimpleMasterDataEntityBaseClassMap(SimpleMasterDataClassMapOptions options = null)
		{
			options ??= new SimpleMasterDataClassMapOptions();

			var cultureFilter = options.MapOnlyDefaultCultureForName
				? (culture, defaultCulture) => culture == defaultCulture
				: default(FilterCulture);

			var invariantIdUniqueKeysJoined = String.Join(", ",
				options.InvariantIdUniqueKeys ?? new[] { "UQ_InvariantId" });

			Not.LazyLoad();

			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.InvariantId).UniqueKey(invariantIdUniqueKeysJoined).Not.Nullable();

			this.DynamicComponentLocalizableString(entity => entity.Name,
				config: options.NameMappingConfig, cultureFilter: cultureFilter);

			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}

	public abstract class SimpleMasterDataEntityBaseRepository<TEntity> : DefaultRepository<TEntity, int>, IQueryByIds<TEntity, int>
		where TEntity : SimpleMasterDataEntityBase
	{

		/// <summary>
		/// Overrides this method if you use DataFilter which should not be applied by QueryByIds().
		/// E.g. IsDeleted entities should come by Id, but not entities outside current user perimeter.
		/// </summary>
		protected virtual IQueryable<TEntity> QueryForQueryByIds()
		{
			return this.Query();
		}

		public IQueryable<TEntity> QueryByIds(int[] ids)
		{
			return this.QueryForQueryByIds().Where(entity => ids.Contains(entity.Id));
		}
	}
}
