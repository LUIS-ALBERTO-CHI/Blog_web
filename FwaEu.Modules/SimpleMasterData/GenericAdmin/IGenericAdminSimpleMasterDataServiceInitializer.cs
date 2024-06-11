using FwaEu.Fwamework;
using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.GenericAdmin
{
	public delegate IPermission GetPermission<TPermissionProvider>(TPermissionProvider provider)
		where TPermissionProvider : IPermissionProvider;

	public interface IGenericAdminSimpleMasterDataServiceInitializer
	{
		IGenericAdminSimpleMasterDataServiceInitializer AddCustomSecurityManager<TManager>()
			where TManager : class, IGenericAdminSimpleMasterDataSecurityManager;

		IGenericAdminSimpleMasterDataServiceInitializer AddPermissionSecurityManager<TPermissionProvider>(
			GetPermission<TPermissionProvider> getPermission)
			where TPermissionProvider : IPermissionProvider;
	}

	public class GenericAdminSimpleMasterDataServiceInitializer : IGenericAdminSimpleMasterDataServiceInitializer
	{
		private readonly ISimpleMasterDataServiceInitializer _inner;

		public GenericAdminSimpleMasterDataServiceInitializer(ISimpleMasterDataServiceInitializer inner)
		{
			this._inner = inner ?? throw new ArgumentNullException(nameof(inner));
		}

		public IServiceCollection ServiceCollection => this._inner.ServiceCollection;
		public ApplicationInitializationContext Context => this._inner.Context;
		public Type EntityType => this._inner.EntityType;
		public Type RepositoryType => this._inner.RepositoryType;

		public IGenericAdminSimpleMasterDataServiceInitializer AddCustomSecurityManager<TManager>()
			where TManager : class, IGenericAdminSimpleMasterDataSecurityManager
		{
			var interfaceType = typeof(IGenericAdminSimpleMasterDataSecurityManagerFactory<>)
				.MakeGenericType(this.EntityType);

			var implementationType = typeof(CustomGenericAdminSimpleMasterDataSecurityManagerFactory<,>)
				.MakeGenericType(this.EntityType, typeof(TManager));

			this._inner.ServiceCollection.AddTransient(interfaceType, implementationType);
			this._inner.ServiceCollection.AddTransient<TManager>();

			return this;
		}

		public IGenericAdminSimpleMasterDataServiceInitializer AddPermissionSecurityManager<TPermissionProvider>(
			GetPermission<TPermissionProvider> getPermission)
				where TPermissionProvider : IPermissionProvider
		{
			var interfaceType = typeof(IGenericAdminSimpleMasterDataSecurityManagerFactory<>)
				.MakeGenericType(this.EntityType);

			var implementationType = typeof(CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManagerFactory<,>)
				.MakeGenericType(this.EntityType, typeof(TPermissionProvider));

			this._inner.ServiceCollection.AddTransient(interfaceType,
				serviceProvider => Activator.CreateInstance(implementationType, getPermission));

			return this;
		}

		public IGenericAdminSimpleMasterDataServiceInitializer AddSecurityManager<TManager>()
			where TManager : class, IGenericAdminSimpleMasterDataSecurityManager
		{
			var interfaceType = typeof(IGenericAdminSimpleMasterDataSecurityManagerFactory<>)
				.MakeGenericType(this.EntityType);

			var implementationType = typeof(CustomGenericAdminSimpleMasterDataSecurityManagerFactory<,>)
				.MakeGenericType(this.EntityType, typeof(TManager));

			this._inner.ServiceCollection.AddTransient(interfaceType, implementationType);
			return this;
		}

		public void InvokeMethod(Type classType, string methodName, Type[] genericTypes = null, params object[] parameters)
		{
			this._inner.InvokeMethod(classType, methodName, genericTypes, parameters);
		}
	}
}
