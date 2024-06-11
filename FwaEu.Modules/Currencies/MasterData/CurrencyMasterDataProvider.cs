using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies.MasterData
{
	public class CurrencyMasterDataProvider : EntityMasterDataProvider
		<CurrencyEntity, int, CurrencyMasterDataModel, CurrencyEntityRepository>
	{
		public CurrencyMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
				: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<CurrencyEntity, CurrencyMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new CurrencyMasterDataModel(entity.Id, entity.IsInverse, entity.CurrencyCode,
				entity.Precision, (string)entity.Name[userCulture.TwoLetterISOLanguageName]
					?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName]);
		}
	}

	public class CurrencyMasterDataModel
	{
		public CurrencyMasterDataModel(int id, bool isInverse, string currencyCode, int precision, string name)
		{
			this.Id = id;
			this.IsInverse = isInverse;
			this.CurrencyCode = currencyCode;
			this.Precision = precision;
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public int Id { get; }
		public bool IsInverse { get; }
		public string CurrencyCode { get; }
		public int Precision { get; }
		public string Name { get; }
	}
}
