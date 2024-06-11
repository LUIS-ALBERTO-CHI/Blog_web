using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.ReportsProvidersByEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.ReportsUserViewsByEntities
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<ReportUserViewEntity>))]
	public class ReportUserViewEntity : IEntity
	{
		public int Id { get; protected set; }
		[LocalizableString]
		public IDictionary Name { get; set; }
		public ReportEntity Report { get; set; }
		public UserEntity User { get; set; }
		public string Value { get; set; }
		public bool IsNew()
		{
			return this.Id == 0;
		}

		public override string ToString()
		{
			return this.Name.ToStringFirstValue();
		}
	}

	public class ReportUserViewEntityClassMap : ClassMap<ReportUserViewEntity>
	{
		public ReportUserViewEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.Report).Not.Nullable();
			References(entity => entity.User);
			Map(entity => entity.Value).Not.Nullable().Length(4001);
			this.DynamicComponentLocalizableString(entity => entity.Name, new LocalizableStringMappingConfig()
			{
				Nullable = LocalizableStringNullable.AllCulturesNotNullable
			});
		}
	}

	public class ReportUserViewEntityRepository : DefaultRepository<ReportUserViewEntity, int>
	{
	
	}
}
