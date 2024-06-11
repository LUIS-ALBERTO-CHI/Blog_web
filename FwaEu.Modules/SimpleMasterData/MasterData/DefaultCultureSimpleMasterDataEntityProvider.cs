using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public sealed class DefaultCultureSimpleMasterDataEntityProvider<TRepository, TEntity>
		: SimpleMasterDataEntityProviderBase<SimpleMasterDataModel, TRepository, TEntity>
			where TEntity : SimpleMasterDataEntityBase
			where TRepository : IRepository<TEntity, int>, IQueryByIds<TEntity, int>
	{
		public DefaultCultureSimpleMasterDataEntityProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<TEntity, SimpleMasterDataModel>> CreateSelectExpression(
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new SimpleMasterDataModel(entity.Id, entity.InvariantId,
				(string)entity.Name[defaultCulture.TwoLetterISOLanguageName]);
		}
	}
}
