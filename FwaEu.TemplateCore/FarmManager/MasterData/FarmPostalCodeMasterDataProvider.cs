using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.TemplateCore.FarmManager.Entities;

namespace FwaEu.TemplateCore.FarmManager.MasterData
{
	public class FarmPostalCodeMasterDataProvider : EntityMasterDataProvider
		<FarmPostalCodeEntity, int, FarmPostalCodeMasterDataModel, FarmPostalCodeEntityRepository>
	{
		public FarmPostalCodeMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
				: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<FarmPostalCodeEntity, FarmPostalCodeMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new FarmPostalCodeMasterDataModel(entity.Id, entity.PostalCode,
					 (string)entity.Town.Name[defaultCulture.TwoLetterISOLanguageName]);
		}
		protected override Expression<Func<FarmPostalCodeEntity, bool>> CreateSearchExpression(string search,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => $"{entity.PostalCode} - {entity.Town.Name}".Contains(search);
		}
	}

	public class FarmPostalCodeMasterDataModel
	{
		public FarmPostalCodeMasterDataModel(int id, string postalCode, string townName)
		{
			this.Id = id;
			this.PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
			this.TownName = townName ?? throw new ArgumentNullException(nameof(townName));
		}

		public int Id { get; }
		public string PostalCode { get; }
		public string TownName { get; }
	}
}

