using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies
{
	public class ExchangeRateEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; protected set; }
		public bool IsInverse { get; set; }
		public string BaseCurrencyCode { get; set; }
		public string QuotedCurrencyCode { get; set; }
		public decimal Value { get; set; }
		public DateTime? Date { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}

		public decimal ToBase(decimal currencyValue)
		{
			var returnValue = Decimal.Zero;
			if (!IsInverse)
				returnValue = currencyValue * Value;
			else
				returnValue = Value == Decimal.Zero ? Decimal.Zero : currencyValue / Value;

			return returnValue;
		}

		public decimal ToQuoted(decimal currencyValue)
		{
			var returnValue = Decimal.Zero;
			if (IsInverse)
				returnValue = currencyValue * Value;
			else
				returnValue = Value == Decimal.Zero ? Decimal.Zero : currencyValue / Value;

			return returnValue;
		}

		public override string ToString()
		{
			return $"{this.BaseCurrencyCode} <=> {this.QuotedCurrencyCode}";
		}
	}

	public class ExchangeRateEntityClassMap : ClassMap<ExchangeRateEntity>
	{
		public ExchangeRateEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.IsInverse).Default("0").Not.Nullable();
			Map(entity => entity.BaseCurrencyCode).Length(10).UniqueKey("ExchangeRateToDate");
			Map(entity => entity.QuotedCurrencyCode).Length(10).UniqueKey("ExchangeRateToDate");
			Map(entity => entity.Value);
			Map(entity => entity.Date).UniqueKey("ExchangeRateToDate");
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}

	public class ExchangeRateEntityRepository : DefaultRepository<ExchangeRateEntity, int>, IQueryByIds<ExchangeRateEntity, int>
	{
		public IQueryable<ExchangeRateEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
	}
}
