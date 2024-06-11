using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Globalization;
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
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<ReportFieldEntity>))]
	public class ReportFieldEntity : ReportDataEntity
	{
		[LocalizableString]
		public IDictionary DisplayFormat { get; set; }

		public string UrlFormat { get; set; }
		public string TemplateName { get; set; }
		public ReportSummaryType SummaryType { get; set; }
	}

	public class ReportFieldEntityClassMap : ReportDataEntityClassMap<ReportFieldEntity>
	{
		public ReportFieldEntityClassMap()
		{
			this.DynamicComponentLocalizableString(entity => entity.DisplayFormat, new LocalizableStringMappingConfig()
			{
				UniqueValueByCulture = false,
				Nullable = LocalizableStringNullable.AllCulturesNullable
			});
			Map(entity => entity.UrlFormat);
			Map(entity => entity.TemplateName);
			Map(entity => entity.SummaryType);
		}
	}

	public class ReportFieldEntityRepository : DefaultRepository<ReportFieldEntity, int>, IQueryByIds<ReportFieldEntity, int>
	{
		public IQueryable<ReportFieldEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}

		public async Task<ReportFieldEntity> GetByInvariantIdAsync(string invariantId)
		{
			return await this.Query().FirstOrDefaultAsync(entity => entity.InvariantId == invariantId);
		}
	}
}
