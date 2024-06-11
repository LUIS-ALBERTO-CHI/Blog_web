using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.TemplateCore.FarmManager.Entities;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.TemplateCore.FarmManager.MasterData
{
	public class FarmTownEntityMasterDataProvider
		: EntityMasterDataProvider<FarmTownEntity, int, FarmTownEntityMasterDataModel, FarmTownEntityRepository>
	{
		public FarmTownEntityMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<FarmTownEntity, FarmTownEntityMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new FarmTownEntityMasterDataModel(entity.Id,
				(string)entity.Name[defaultCulture.TwoLetterISOLanguageName]);
		}

		protected override Expression<Func<FarmTownEntity, bool>> CreateSearchExpression(string search,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => ((string)entity.Name[defaultCulture.TwoLetterISOLanguageName]).Contains(search);
		}
	}

	public class FarmTownEntityMasterDataModel
	{
		public FarmTownEntityMasterDataModel(int id, string name)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public int Id { get; }
		public string Name { get; }
	}
}