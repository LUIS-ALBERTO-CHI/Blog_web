using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public abstract class SimpleMasterDataEntityProviderBase<TBaseMasterDataModel, TRepository, TEntity>
		: EntityMasterDataProvider<TEntity, int, TBaseMasterDataModel, TRepository>
			where TEntity : SimpleMasterDataEntityBase
			where TRepository : IRepository<TEntity, int>, IQueryByIds<TEntity, int>
			where TBaseMasterDataModel : SimpleMasterDataModel
	{
		protected SimpleMasterDataEntityProviderBase(
			BaseSessionContext<IStatefulSessionAdapter> sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<TEntity, bool>> CreateSearchExpression(
			string search, CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => ((string)entity.Name[userCulture.TwoLetterISOLanguageName]
				?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName]).Contains(search);
		}
	}
}
