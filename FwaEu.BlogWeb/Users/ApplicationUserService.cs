using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users
{
	public class ApplicationUserService : UserServiceBase<ApplicationUserEntity,
		ApplicationUserLoadingModel, ApplicationUserLoadingListModel,
		ApplicationUserEntityRepository>
	{
		public ApplicationUserService(
			UserSessionContext userSessionContext,
			IPartServiceFactory partServiceFactory,
			IListPartServiceFactory listPartServiceFactory,
			ICurrentUserService currentUserService)
			: base(userSessionContext, partServiceFactory,
				  listPartServiceFactory, currentUserService)
		{
		}

		protected override Expression<Func<ApplicationUserEntity, ApplicationUserLoadingListModel>> CreateSelectLoadingListModelExpression()
		{
			return user => new ApplicationUserLoadingListModel()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Identity = user.Identity,
				State = user.State,
				UpdatedById = user.UpdatedBy.Id,
				UpdatedOn = user.UpdatedOn,
				
				FarmerPseudonym = user.FarmerPseudonym,
			};
		}

		protected override Expression<Func<ApplicationUserEntity, ApplicationUserLoadingModel>> CreateSelectLoadingModelExpression()
		{
			return user => new ApplicationUserLoadingModel()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				IsAdmin = user.IsAdmin,
				State = user.State,

				CreatedById = user.CreatedBy.Id,
				CreatedOn = user.CreatedOn,
				UpdatedById = user.UpdatedBy.Id,
				UpdatedOn = user.UpdatedOn,

				FarmerPseudonym = user.FarmerPseudonym,
			};
		}
	}
}