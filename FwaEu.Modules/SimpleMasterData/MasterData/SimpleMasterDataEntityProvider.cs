using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public sealed class SimpleMasterDataEntityProvider<TRepository, TEntity>
		: SimpleMasterDataEntityProviderBase<SimpleMasterDataModel, TRepository, TEntity>
			where TEntity : SimpleMasterDataEntityBase
			where TRepository : IRepository<TEntity, int>, IQueryByIds<TEntity, int>
	{
		public SimpleMasterDataEntityProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<TEntity, SimpleMasterDataModel>> CreateSelectExpression(
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new SimpleMasterDataModel(entity.Id, entity.InvariantId,
				(string)entity.Name[userCulture.TwoLetterISOLanguageName]
					?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName]);
		}
	}
}
