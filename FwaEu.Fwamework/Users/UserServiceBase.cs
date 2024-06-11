using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users.Parts;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public abstract class UserServiceBase<TEntity, TLoadingModel, TLoadingListModel, TRepository> : IUserService
		where TEntity : UserEntity, new()
		where TLoadingModel : UserEditModel
		where TLoadingListModel : UserListModel
		where TRepository : DefaultUserEntityRepository<TEntity>

	{
		public UserServiceBase(
			UserSessionContext userSessionContext,
			IPartServiceFactory partServiceFactory,
			IListPartServiceFactory listPartServiceFactory,
			ICurrentUserService currentUserService)
		{
			this.UserSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this.PartServiceFactory = partServiceFactory
				?? throw new ArgumentNullException(nameof(partServiceFactory));

			this.ListPartServiceFactory = listPartServiceFactory
				?? throw new ArgumentNullException(nameof(listPartServiceFactory));

			this.CurrentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));
		}

		protected UserSessionContext UserSessionContext { get; }
		protected IRepositoryFactory RepositoryFactory { get; }
		protected IPartServiceFactory PartServiceFactory { get; }
		protected IListPartServiceFactory ListPartServiceFactory { get; }
		protected ICurrentUserService CurrentUserService { get; }

		protected abstract Expression<Func<TEntity, TLoadingModel>> CreateSelectLoadingModelExpression();
		protected abstract Expression<Func<TEntity, TLoadingListModel>> CreateSelectLoadingListModelExpression();

		private async Task<IEnumerable<UserListModel>> GetAllInternalAsync(IQueryable<TEntity> query)
		{
			var loadingModels = await query
				.Select(this.CreateSelectLoadingListModelExpression())
				.ToListAsync();

			var service = this.ListPartServiceFactory.Create();

			foreach (var loadingModel in loadingModels)
			{
				var parts = await service.LoadAllPartsAsync(loadingModel);
				loadingModel.Parts = parts;
			}

			return loadingModels;
		}

		public async Task<IEnumerable<UserListModel>> GetAllAsync()
		{
			return await GetAllInternalAsync(this.UserSessionContext.SessionContext
				.RepositorySession.Create<TRepository>()
				.Query());
		}

		public async Task<IEnumerable<UserListModel>> GetAllForAdminAsync()
		{
			return await GetAllInternalAsync(this.UserSessionContext.SessionContext
				.RepositorySession.Create<TRepository>()
				.QueryForUsersAdmin());
		}

		public async Task<UserEditModel> GetInternalAsync(int id, IQueryable<TEntity> query)
		{
			var loadingModel = await query
					.Where(user => user.Id == id)
					.Select(this.CreateSelectLoadingModelExpression())
				.FirstOrDefaultAsync();

			if (loadingModel == null)
			{
				throw new UserNotFoundException($"User not found with id #{id}.");
			}

			this.UserSessionContext.LoadingModel = loadingModel;

			var parts = await this.PartServiceFactory.Create().LoadAllPartsAsync();
			loadingModel.Parts = parts;

			return loadingModel;
		}

		public async Task<UserEditModel> GetForAdminAsync(int id)
		{
			return await GetInternalAsync(id, this.UserSessionContext.SessionContext.RepositorySession
				.Create<TRepository>().QueryForUsersAdmin());
		}

		public async Task<UserEditModel> GetAsync(int id)
		{
			return await GetInternalAsync(id, this.UserSessionContext.SessionContext.RepositorySession
				.Create<TRepository>().Query());
		}

		public async Task<int> SaveAsync(UserSaveModel user)
		{
			var userSessionContext = this.UserSessionContext;
			var repositorySession = userSessionContext.SessionContext.RepositorySession;

			using (var transaction = repositorySession.Session.BeginTransaction())
			{
				var repository = repositorySession.Create<TRepository>();

				var entity = user.Id == 0 ? new TEntity():
								await repositorySession.GetOrNotFoundExceptionAsync<TEntity, int, TRepository>(user.Id);

				userSessionContext.SaveUserEntity = entity;

				var partService = this.PartServiceFactory.Create();

				var result = await partService.SaveAllPartsAsync(user.Parts);
				await repository.SaveOrUpdateAsync(entity);
				await result.FinalizeAfterEntitySaveAsync();

				await repositorySession.Session.FlushAsync();
				await transaction.CommitAsync();

				await result.FinalizeAfterTransactionAsync();

				return entity.Id;
			}
		}

		public Dictionary<string, IPartHandler> GetSaveHandlerByPartName()
		{
			return this.PartServiceFactory.Create().GetSaveHandlerByPartName();
		}
	}
}
