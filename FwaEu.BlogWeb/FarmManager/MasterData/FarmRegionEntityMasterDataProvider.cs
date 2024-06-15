using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.BlogWeb.FarmManager.MasterData
{
	public class TownRegionEntityMasterDataProvider
		: EntityMasterDataProvider<TownRegionEntity, int, TownRegionEntityMasterDataModel, TownRegionEntityRepository>
	{
		public TownRegionEntityMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<TownRegionEntity, TownRegionEntityMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new TownRegionEntityMasterDataModel(entity.Id,
				(string)entity.Name[defaultCulture.TwoLetterISOLanguageName]);
		}

		protected override Expression<Func<TownRegionEntity, bool>> CreateSearchExpression(string search,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => ((string)entity.Name[defaultCulture.TwoLetterISOLanguageName]).Contains(search);
		}
	}

	public class TownRegionEntityMasterDataModel
	{
		public TownRegionEntityMasterDataModel(int id, string name)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public int Id { get; }
		public string Name { get; }
	}
}