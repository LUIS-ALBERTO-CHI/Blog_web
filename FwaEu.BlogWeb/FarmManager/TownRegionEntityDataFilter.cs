using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager
{
	public class TownRegionEntityDataFilter : IRepositoryDataFilter<TownRegionEntity, int>
	{
		public Expression<Func<TownRegionEntity, bool>> Accept(RepositoryDataFilterContext<TownRegionEntity, int> context)
		{
			var currentUserEntity = default(UserEntity);

			if (context.ServiceProvider.GetRequiredService<ICurrentUserPerimeterService>().FullAccessKeys
					.Contains(TownRegionPerimeterEntity.ProviderKey)
				|| (currentUserEntity = context.ServiceProvider.GetRequiredService<ICurrentUserService>()?.User?.Entity) == null)
			{
				return null;
			}

			var regionPerimeterRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<TownRegionPerimeterEntityRepository>(context.Session);
			return region => regionPerimeterRepository.QueryNoPerimeter()
				.Where(rp => rp.User == currentUserEntity)
				.Any(p => p.Region == region);
		}

		public async Task<bool> AcceptAsync(TownRegionEntity entity, RepositoryDataFilterContext<TownRegionEntity, int> context)
		{
			var currentUserEntity = default(UserEntity);

			if (context.ServiceProvider.GetRequiredService<ICurrentUserPerimeterService>().FullAccessKeys
					.Contains(TownRegionPerimeterEntity.ProviderKey)
				|| (currentUserEntity = context.ServiceProvider.GetRequiredService<ICurrentUserService>()?.User?.Entity) == null)
			{
				return true;
			}

			var regionPerimeterRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<TownRegionPerimeterEntityRepository>(context.Session);

			return await regionPerimeterRepository.QueryNoPerimeter()
				.Where(rp => rp.User == currentUserEntity)
				.AnyAsync(p => p.Region == entity);
		}
	}
}
