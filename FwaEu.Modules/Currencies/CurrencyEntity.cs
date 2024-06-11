using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<CurrencyEntity>))]
	public class CurrencyEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; protected set; }
		public bool IsInverse { get; set; }
		public string CurrencyCode { get; set; }
		public int Precision { get; set; }

		[LocalizableString]
		public IDictionary Name { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}

		public override string ToString()
		{
			return this.CurrencyCode;
		}
	}

	public class CurrencyEntityClassMap : ClassMap<CurrencyEntity>
	{
		public CurrencyEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.IsInverse).Default("0").Not.Nullable();
			Map(entity => entity.CurrencyCode).Length(10).Unique().Not.Nullable();
			Map(entity => entity.Precision).Not.Nullable();
			this.DynamicComponentLocalizableString(entity => entity.Name);
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}

	public class CurrencyEntityRepository : DefaultRepository<CurrencyEntity, int>, IQueryByIds<CurrencyEntity, int>
	{
		public async Task<CurrencyEntity> GetByCodeAsync(string code)
		{
			return await this.Query().FirstOrDefaultAsync(currency => currency.CurrencyCode == code);
		}

		public IQueryable<CurrencyEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
	}
}
