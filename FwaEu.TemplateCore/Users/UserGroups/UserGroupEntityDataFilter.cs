using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users.UserGroups
{
	public class UserGroupEntityDataFilter : IRepositoryDataFilter<UserGroupEntity, int>
	{
		public Expression<Func<UserGroupEntity, bool>> Accept(RepositoryDataFilterContext<UserGroupEntity, int> context)
		{
			var currentUserEntity = default(UserEntity);

			if (context.ServiceProvider.GetRequiredService<ICurrentUserPerimeterService>().FullAccessKeys
					.Contains(UserGroupPerimeterEntity.ProviderKey)
				|| (currentUserEntity = context.ServiceProvider.GetRequiredService<ICurrentUserService>()?.User?.Entity) == null)
			{
				return null;
			}

			var userGroupPerimeterRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<UserGroupPerimeterEntityRepository>(context.Session);

			return ug => userGroupPerimeterRepository.QueryNoPerimeter()
				.Where(ugp => ugp.User == currentUserEntity)
				.Any(ugp => ugp.UserGroup == ug);
		}

		public async Task<bool> AcceptAsync(UserGroupEntity entity, RepositoryDataFilterContext<UserGroupEntity, int> context)
		{
			var currentUserEntity = default(UserEntity);

			if (context.ServiceProvider.GetRequiredService<ICurrentUserPerimeterService>().FullAccessKeys
					.Contains(UserGroupPerimeterEntity.ProviderKey)
				|| (currentUserEntity = context.ServiceProvider.GetRequiredService<ICurrentUserService>()?.User?.Entity) == null)
			{
				return true;
			}

			var userGroupPerimeterRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<UserGroupPerimeterEntityRepository>(context.Session);

			return await userGroupPerimeterRepository.QueryNoPerimeter()
				.Where(ugp => ugp.User == currentUserEntity)
				.AnyAsync(ugp => ugp.UserGroup == entity);
		}
	}
}
