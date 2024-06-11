using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface IPerimeterEntity<TReferenceEntityId> : IEntity
	{
		TReferenceEntityId GetReferenceEntityId(); // TODO: Remove https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3911
		UserEntity User { get; set; }
	}

	public abstract class EntityUserPerimeterProviderBase
			<TEntity, TRepository, TReferenceEntity, TReferenceEntityId> : IUserPerimeterProvider
		where TEntity : class, IPerimeterEntity<TReferenceEntityId>
		where TRepository : IRepository<TEntity>
		where TReferenceEntity : class
	{
		protected EntityUserPerimeterProviderBase(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.SessionAdapterFactory = serviceProvider.GetRequiredService<ISessionAdapterFactory>();
			this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
		}

		protected IServiceProvider ServiceProvider { get; }
		protected ISessionAdapterFactory SessionAdapterFactory { get; }
		protected IRepositoryFactory RepositoryFactory { get; }

		protected abstract Expression<Func<TEntity, bool>> WhereFullAccess();

		// TODO: Create a virtual implementation based on WhereFullAccess https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3911
		protected abstract Expression<Func<TEntity, bool>> WhereNotFullAccess();

		// TODO: Create a virtual implementation based on SelectReferenceEntityId https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3911
		protected abstract Expression<Func<TEntity, bool>> WhereContainsReferenceEntities(TReferenceEntityId[] ids);

		protected abstract Expression<Func<TEntity, TReferenceEntityId>> SelectReferenceEntityId();

		// TODO: Create a virtual implementation based on SelectReferenceEntityId https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3911
		protected abstract TEntity CreatePerimeterEntity(TReferenceEntity referenceEntity);

		private TEntity CreatePerimeterEntityAndSetUser(TReferenceEntity referenceEntity, UserEntity user)
		{
			var entity = this.CreatePerimeterEntity(referenceEntity);
			entity.User = user;
			return entity;
		}

		private async Task<List<TReferenceEntityId>> GetAccessibleIdsAsync(int userId, TRepository repository)
		{
			return await repository
					.QueryNoPerimeter() //NOTE: QueryNoPerimeter(), the Query() of the perimeter entity shouldn't be overrided in the repository anyway
					.Where(e => e.User.Id == userId)
					.Where(this.WhereNotFullAccess())
					.Select(this.SelectReferenceEntityId())
					.ToListAsync();
		}

		public async Task<TReferenceEntityId[]> GetAccessibleIdsAsync(int userId)
		{
			using (var session = this.SessionAdapterFactory.CreateStatefulSession())
			{
				return (await this.GetAccessibleIdsAsync(userId,
					this.RepositoryFactory.Create<TRepository>(session)))
					.ToArray();
			}
		}

		private async Task<bool> HasFullAccessAsync(int userId, TRepository repository)
		{
			return await repository.QueryNoPerimeter()
				.Where(e => e.User.Id == userId)
				.Where(this.WhereFullAccess())
				.WithOptions(options =>
					options.SetCacheable(true)
					.SetCacheRegion("Perimeters"))
				.AnyAsync();
		}

		protected async virtual Task<bool> HasFullAccessFallbackAsync(int userId, IStatefulSessionAdapter session)
		{
			return await this.RepositoryFactory.Create<IUserEntityRepository>(session)
				.IsAdminAsync(userId);
		}

		public async Task<bool> HasFullAccessAsync(int userId)
		{
			using (var session = this.SessionAdapterFactory.CreateStatefulSession())
			{
				var hasFullAccess = await this.HasFullAccessAsync(userId,
					this.RepositoryFactory.Create<TRepository>(session));

				if (!hasFullAccess)
				{
					return await this.HasFullAccessFallbackAsync(userId, session);
				}

				return hasFullAccess;
			}
		}

		private async Task<(bool ConsiderLikeFullAccess, TReferenceEntityId[] Perimeter)> GetCallingContextAsync(TRepository repository)
		{
			var currentUser = this.ServiceProvider.GetRequiredService<ICurrentUserService>().User;

			var currentUserHasFullAccess = currentUser == null ? true
				: currentUser.Entity.IsAdmin ? true //NOTE: Admins can grant or remove full access to anybody
					: await this.HasFullAccessAsync(currentUser.Entity.Id, repository);

			if (currentUserHasFullAccess)
			{
				return (true, null);
			}

			var currentUserPerimeter = await this.GetAccessibleIdsAsync(currentUser.Entity.Id, repository);
			return (false, currentUserPerimeter.ToArray());
		}

		private async Task DoUpdatePerimeterAsync(UserEntity user,
			IStatefulSessionAdapter session, bool hasFullAccess,
			TReferenceEntityId[] accessibleIds)
		{
			var repository = this.RepositoryFactory.Create<TRepository>(session);

			var currentPerimeterQuery = repository
				.QueryNoPerimeter() //NOTE: QueryNoPerimeter(), the Query() of the perimeter entity shouldn't be overrided in the repository anyway
				.Where(e => e.User == user);

			var callingContext = await GetCallingContextAsync(repository);

			var currentPerimeter = await currentPerimeterQuery.ToListAsync();
			var currentFullAccess = currentPerimeter.FirstOrDefault(
				this.WhereFullAccess().Compile());

			if (hasFullAccess)
			{
				if (!callingContext.ConsiderLikeFullAccess)
				{
					throw new UserPerimeterException("Full access can only be granted if the current user has full access.");
				}

				var whereNotFullAccess = this.WhereNotFullAccess();
				if (currentPerimeter.Any(whereNotFullAccess.Compile()))
				{
					await currentPerimeterQuery
						.Where(whereNotFullAccess)
						.DeleteAsync(CancellationToken.None);
				}

				if (currentFullAccess == null)
				{
					var entity = this.CreatePerimeterEntityAndSetUser(null, user);
					await repository.SaveOrUpdateAsync(entity);
				}
			}
			else
			{
				if (currentFullAccess != null)
				{
					if (!callingContext.ConsiderLikeFullAccess)
					{
						throw new UserPerimeterException("Full access can only be removed if the current user has full access.");
					}

					await repository.DeleteAsync(currentFullAccess);
					currentPerimeter.Remove(currentFullAccess);
				}

				if (accessibleIds.Any())
				{
					var ids = accessibleIds.ToArray();

					if (callingContext.Perimeter != null) //NOTE: If current user has a perimeter, he can't grant or remove access to ids outside his own perimeter
					{
						if (ids.Join(callingContext.Perimeter, id => id, ccid => ccid, (id, ccid) => id)
							.Count() != ids.Length)
						{
							throw new UserPerimeterException(
								"Trying to grant or remove access to entities outside the current user perimeter is not allowed.");
						}
					}

					var existing = currentPerimeter.Join(
						ids, cp => cp.GetReferenceEntityId(), id => id, (cp, id) => id).ToArray();

					var toCreate = ids.Except(existing).ToArray();

					var toDelete = currentPerimeter
						.Select(cp => cp.GetReferenceEntityId())
						.Except(ids).ToArray();

					if (toDelete.Any() || toCreate.Any())
					{
						var referenceEntityService = this.CreateReferenceEntityService(session);

						if (toDelete.Any())
						{
							await currentPerimeterQuery
								.Where(this.WhereContainsReferenceEntities(toDelete))
								.DeleteAsync(CancellationToken.None);
						}

						foreach (var id in toCreate)
						{
							var referenceEntity = await referenceEntityService.GetAsync(id);

							var entity = this.CreatePerimeterEntityAndSetUser(referenceEntity, user);
							await repository.SaveOrUpdateAsync(entity);
						}
					}
				}
			}

			await session.FlushAsync();
		}

		protected abstract IReferenceEntityService<
			TReferenceEntity, TReferenceEntityId> CreateReferenceEntityService(ISessionAdapter session);

		public async Task UpdatePerimeterAsync(int userId,
			bool hasFullAccess, TReferenceEntityId[] accessibleIds)
		{
			var userSessionContext = this.ServiceProvider.GetRequiredService<UserSessionContext>();
			var externalSession = userSessionContext.SessionContext.RepositorySession.Session;

			if (externalSession == null)
			{
				using (var session = this.SessionAdapterFactory.CreateStatefulSession())
				{
					var user = await this.RepositoryFactory.Create<IUserEntityRepository>(session)
						.GetAsync(userId);

					await this.DoUpdatePerimeterAsync(user,
					   session, hasFullAccess, accessibleIds);
				}
			}
			else
			{
				var user = userSessionContext.SaveUserEntity;

				if (user == null)
				{
					throw new NotSupportedException(
						$"The service store must provide the {nameof(UserEntity)}.");
				}

				await this.DoUpdatePerimeterAsync(user,
				   externalSession, hasFullAccess, accessibleIds);
			}
		}

		async Task<object[]> IUserPerimeterProvider.GetAccessibleIdsAsync(int userId)
		{
			return (await this.GetAccessibleIdsAsync(userId))
				.Cast<object>().ToArray();
		}

		Task IUserPerimeterProvider.UpdatePerimeterAsync(int userId, bool hasFullAccess,
			object[] accessibleIds)
		{
			return this.UpdatePerimeterAsync(userId, hasFullAccess,
				accessibleIds == null ? null : accessibleIds
					.Select(id => Convert.ChangeType(id, typeof(TReferenceEntityId)))
					.Cast<TReferenceEntityId>().ToArray());
		}
	}

	public abstract class EntityUserPerimeterProviderBase
			<TEntity, TRepository, TReferenceEntity, TReferenceEntityId, TReferenceEntityRepository>
			: EntityUserPerimeterProviderBase<TEntity, TRepository, TReferenceEntity, TReferenceEntityId>
		where TEntity : class, IPerimeterEntity<TReferenceEntityId>
		where TRepository : IRepository<TEntity>
		where TReferenceEntity : class
		where TReferenceEntityRepository : IRepository<TReferenceEntity, TReferenceEntityId>
	{
		public EntityUserPerimeterProviderBase(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override IReferenceEntityService<
			TReferenceEntity, TReferenceEntityId> CreateReferenceEntityService(ISessionAdapter session)
		{
			return new RepositoryReferenceEntityService<TReferenceEntity,
				TReferenceEntityId, TReferenceEntityRepository>
				(this.RepositoryFactory, session);
		}
	}
}
