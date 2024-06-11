using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.Tests.MasterData
{
	public class MasterDataEntityMasterDataProvider : EntityMasterDataProvider<MasterDataEntity, int, MasterDataModel, MasterDataEntityRepository>
	{
		public MasterDataEntityMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<MasterDataEntity, MasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new MasterDataModel()
			{
				Id = entity.Id,
				Name = entity.Name
			};
		}

		protected override Expression<Func<MasterDataEntity, bool>> CreateSearchExpression(string search, CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => entity.Name.Contains(search);
		}
	}

	public class MasterDataModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
