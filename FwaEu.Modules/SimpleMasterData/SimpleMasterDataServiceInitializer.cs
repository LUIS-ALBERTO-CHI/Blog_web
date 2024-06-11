using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData
{
	public interface ISimpleMasterDataServiceInitializer
	{
		IServiceCollection ServiceCollection { get; }
		ApplicationInitializationContext Context { get; }

		Type EntityType { get; }
		Type RepositoryType { get; }

		void InvokeMethod(Type classType, string methodName,
			Type[] genericTypes = null, params object[] parameters);
	}

	public class SimpleMasterDataServiceInitializer<TEntity, TRepository> : ConstructSimpleMasterDataServiceInitializer<TEntity>, ISimpleMasterDataServiceInitializer
		where TEntity : SimpleMasterDataEntityBase
		where TRepository : class, IRepository, IRepository<TEntity, int>, IQueryByIds<TEntity, int>, new()
	{
		public SimpleMasterDataServiceInitializer(IServiceCollection serviceCollection,
			ApplicationInitializationContext context)
			: base(serviceCollection, context)
		{
		}

		public Type RepositoryType => typeof(TRepository);
	}

	public class ConstructSimpleMasterDataServiceInitializer<TEntity>
		where TEntity : SimpleMasterDataEntityBase
	{
		public ConstructSimpleMasterDataServiceInitializer(
			IServiceCollection serviceCollection,
			ApplicationInitializationContext context)
		{
			this.ServiceCollection = serviceCollection
				?? throw new ArgumentNullException(nameof(serviceCollection));

			this.Context = context
				?? throw new ArgumentNullException(nameof(context));
		}

		public IServiceCollection ServiceCollection { get; }
		public ApplicationInitializationContext Context { get; }

		public Type EntityType => typeof(TEntity);

		public ConstructSimpleMasterDataServiceInitializer<TEntity> AddCustomKeyResolver<TKeyResolver>()
			where TKeyResolver : class, IEntityKeyResolver<TEntity>
		{
			this.ServiceCollection.AddTransient<IEntityKeyResolver<TEntity>, TKeyResolver>();
			return this;
		}

		public SimpleMasterDataServiceInitializer<TEntity, TRepository> AddRepository<TRepository>()
			where TRepository : class, IRepository, IRepository<TEntity, int>, IQueryByIds<TEntity, int>, new()
		{
			var repositoryRegister = this.Context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<TRepository>();

			return new SimpleMasterDataServiceInitializer<TEntity, TRepository>(
				this.ServiceCollection, this.Context);
		}

		public SimpleMasterDataServiceInitializer<TEntity, TRepository>
			AddRepository<TEntityImplementation, TRepository, TRepositoryImplementation>()
				where TEntityImplementation : class
				where TRepository : class, IRepository, IRepository<TEntity, int>, IQueryByIds<TEntity, int>, new()
				where TRepositoryImplementation : class, TRepository, new()
		{
			var repositoryRegister = this.Context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<TEntity, TEntityImplementation, TRepository, TRepositoryImplementation>();

			return new SimpleMasterDataServiceInitializer<TEntity, TRepository>(
				this.ServiceCollection, this.Context);
		}

		public void InvokeMethod(Type classType, string methodName,
			Type[] genericTypes = null, params object[] parameters)
		{
			var allParameters = (IEnumerable<object>)new object[] { this };

			if (parameters != null)
			{
				allParameters = allParameters.Concat(parameters);
			}

			var method = classType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

			if (genericTypes != null)
			{
				method = method.MakeGenericMethod(genericTypes);
			}

			method.Invoke(null, allParameters.ToArray());
		}
	}
}
