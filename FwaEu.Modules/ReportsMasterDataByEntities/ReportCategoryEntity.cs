using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<ReportCategoryEntity>))]
	public class ReportCategoryEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; protected set; }
		public string InvariantId { get; set; }

		[LocalizableString]
		public IDictionary Name { get; set; }

		[LocalizableString]
		public IDictionary Description { get; set; }

		public DateTime UpdatedOn { get; set; }
		public bool IsNew()
		{
			return this.Id == 0;
		}
		public override string ToString()
		{
			return this.Name.ToStringFirstValue();
		}
	}

	public class ReportCategoryEntityClassMap : ClassMap<ReportCategoryEntity>
	{
		public ReportCategoryEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.InvariantId).Not.Nullable();
			this.DynamicComponentLocalizableString(entity => entity.Name, new LocalizableStringMappingConfig()
			{
				Nullable = LocalizableStringNullable.AllCulturesNotNullable
			});
			this.DynamicComponentLocalizableString(entity => entity.Description, new LocalizableStringMappingConfig()
			{
				Nullable = LocalizableStringNullable.AllCulturesNullable
			});
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}

	public class ReportCategoryEntityRepository : DefaultRepository<ReportCategoryEntity, int>, IQueryByIds<ReportCategoryEntity, string>
	{
		public IQueryable<ReportCategoryEntity> QueryByIds(string[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.InvariantId));
		}
	}
}
