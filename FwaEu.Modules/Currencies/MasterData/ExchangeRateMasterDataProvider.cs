using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies.MasterData
{
	public class ExchangeRateMasterDataProvider : EntityMasterDataProvider<ExchangeRateEntity, int,
		ExchangeRateMasterDataModel, ExchangeRateEntityRepository>
	{
		public ExchangeRateMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
				: base(sessionContext, culturesService)
		{
		}

		protected override System.Linq.Expressions.Expression<Func<ExchangeRateEntity, ExchangeRateMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new ExchangeRateMasterDataModel(entity.Id, entity.IsInverse, entity.BaseCurrencyCode,
				entity.QuotedCurrencyCode, entity.Value, entity.Date);
		}
	}

	public class ExchangeRateMasterDataModel
	{
		public ExchangeRateMasterDataModel(int id, bool isInverse, string baseCurrencyCode, string quotedCurrencyCode,
			decimal value, DateTime? date)
		{
			this.Id = id;
			this.IsInverse = isInverse;
			this.BaseCurrencyCode = baseCurrencyCode ?? throw new ArgumentNullException(nameof(baseCurrencyCode));
			this.QuotedCurrencyCode = quotedCurrencyCode ?? throw new ArgumentNullException(nameof(quotedCurrencyCode));
			this.Value = value;
			this.Date = date;
		}

		public int Id { get; }
		public bool IsInverse { get; }
		public string BaseCurrencyCode { get; }
		public string QuotedCurrencyCode { get; }
		public decimal Value { get; }
		public DateTime? Date { get; }
	}
}

