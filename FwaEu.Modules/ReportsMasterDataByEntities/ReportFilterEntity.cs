using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.ReportsMasterDataByEntities;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public abstract class ReportDataEntity : IUpdateTracked
	{
		public int Id { get; protected set; }
		public string InvariantId { get; set; }
		[LocalizableString]
		public IDictionary Name { get; set; }
		public string DotNetTypeName { get; set; }
		public string DataSourceType { get; set; }
		public string DataSourceArgument { get; set; }
		public DateTime UpdatedOn { get; set; }
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

	public abstract class ReportDataEntityClassMap<TEntity> : ClassMap<TEntity>
	where TEntity : ReportDataEntity
	{
		public ReportDataEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.InvariantId).Not.Nullable();
			Map(entity => entity.DotNetTypeName).Not.Nullable();
			Map(entity => entity.DataSourceType).Nullable();
			Map(entity => entity.DataSourceArgument).Length(40000);
			this.AddUpdateTrackedPropertiesIntoMapping();
			this.DynamicComponentLocalizableString(entity => entity.Name, new LocalizableStringMappingConfig()
			{
				Nullable = LocalizableStringNullable.AllCulturesNotNullable
			});
		}
	}

	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<ReportFilterEntity>))]
	public class ReportFilterEntity : ReportDataEntity
	{
		
	}

	public class ReportFilterEntityClassMap : ReportDataEntityClassMap<ReportFilterEntity>
	{

	}

	public class ReportFilterEntityRepository : DefaultRepository<ReportFilterEntity, int>, IQueryByIds<ReportFilterEntity, int>
	{
		public IQueryable<ReportFilterEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
		public async Task<ReportFilterEntity> GetByInvariantIdAsync(string invariantId)
		{
			return await this.Query().FirstOrDefaultAsync(entity => entity.InvariantId == invariantId);
		}
	}
}
