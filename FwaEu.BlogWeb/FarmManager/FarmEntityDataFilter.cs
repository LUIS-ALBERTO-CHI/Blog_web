using FwaEu.Fwamework.Data.Database;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager
{
	public class FarmEntityDataFilter : IRepositoryDataFilter<FarmEntity, int>
	{

		public Expression<Func<FarmEntity, bool>> Accept(RepositoryDataFilterContext<FarmEntity, int> context)
		{
			var contextService = context.ServiceProvider.GetRequiredService<IViewContextService>();
			var farmRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<FarmEntityRepository>(context.Session);
			if (contextService.Current?.Region != null)
			{

				return farm => farmRepository.QueryNoPerimeter()
				.Where(f => f.PostalCode.Town.Region.Id == contextService.Current.Region.Id)
				.Any(p => p == farm);
			}
			return null;
		}

		public async Task<bool> AcceptAsync(FarmEntity entity, RepositoryDataFilterContext<FarmEntity, int> context)
		{
			var contextService = context.ServiceProvider.GetRequiredService<IViewContextService>();

			var farmEntityRepository = context.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<FarmEntityRepository>(context.Session);
			if (contextService.Current?.Region != null)
			{
				return await farmEntityRepository.QueryNoPerimeter()
					.Where(f => f.PostalCode.Town.Region.Id == contextService.Current.Region.Id)
					.AnyAsync(p => p == entity);
			};
			return true;
		}
	}
}
