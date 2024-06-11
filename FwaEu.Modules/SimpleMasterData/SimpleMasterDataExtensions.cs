using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData
{
	public static class SimpleMasterDataExtensions
	{
		public static ConstructSimpleMasterDataServiceInitializer<TEntity>
			For<TEntity>(this IServiceCollection serviceCollection, ApplicationInitializationContext context)
				where TEntity : SimpleMasterDataEntityBase
		{
			
			return new ConstructSimpleMasterDataServiceInitializer<TEntity>(serviceCollection, context);
		}
	}

	
}
