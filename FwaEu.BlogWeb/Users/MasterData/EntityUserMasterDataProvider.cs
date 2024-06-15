using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Users.Avatars;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.MasterData
{
	public class EntityUserMasterDataProvider
		: EntityMasterDataProvider<ApplicationUserEntity, int, EntityUserMasterDataModel, ApplicationUserEntityRepository>
	{
		public EntityUserMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<ApplicationUserEntity, EntityUserMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new EntityUserMasterDataModel(entity.Id,
				entity.FirstName, entity.LastName, entity.Email);
		}

		protected override Expression<Func<ApplicationUserEntity, bool>> CreateSearchExpression(string search,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => (entity.FirstName + " " + entity.LastName).Contains(search);
		}
	}
}
