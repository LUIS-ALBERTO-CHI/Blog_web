using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.Import
{
	public class ApplicationUserImporter : ModelImporter<ApplicationUserImportModel, IEntityImporterEventListener>
	{
		public ApplicationUserImporter(IServiceProvider serviceProvider,
			IModelBinder<ApplicationUserImportModel> modelBinder,
			UserSessionContext userSessionContext,
			CurrentUserPermissionService currentUserPermissionService)
			: base(serviceProvider, modelBinder)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		private async Task<bool> IsImportAuthorizedAsync()
		{
			var user = this._currentUserPermissionService.CurrentUserService.User?.Entity;

			if (user == null) // NOTE: Case of import in SetupTask
			{
				return true;
			}

			return await this._currentUserPermissionService.HasPermissionAsync<UsersPermissionProvider>(
				provider => provider.CanAdministrateUsers);
		}


		protected override async Task ImportAsync(DataReader reader, ModelImporterContext context, ProcessResultContext processResultContext)
		{
			var isImportAuthorized = await this.IsImportAuthorizedAsync();

			if (isImportAuthorized)
			{
				// HACK: Used by TransactionEntityImporterEventListener
				context.ServiceStore.Add<IStatefulSessionAdapter>(this._userSessionContext.SessionContext.RepositorySession.Session);
				await base.ImportAsync(reader, context, processResultContext);
			}
			else
			{
				processResultContext.Add(new ErrorProcessResultEntry("Not authorized."));
			}
		}

		protected override async Task<ModelLoadResult<ApplicationUserImportModel>> LoadModelAsync(
			ModelPropertyDescriptor[] keys, DataRow row, ModelImporterContext context)
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;

			var entity = await repositorySession
				.Create<ApplicationUserEntityRepository>()
				.FindByIdentityAsync((string)row.ValuesByPropertyName[nameof(ApplicationUserEntity.Email)]);

			var model = new ApplicationUserImportModel((ApplicationUserEntity)entity, row);
			return new ModelLoadResult<ApplicationUserImportModel>(model, entity == null);
		}

		protected override async Task SaveModelAsync(ApplicationUserImportModel model, ModelImporterContext context)
		{
			var userEntity = model.Entity ?? new ApplicationUserEntity() { Email = model.Email };
			this._userSessionContext.SaveUserEntity = userEntity;

			// NOTE: model.DataRow.MetadataProperties contains columns which are in the XLS file.
			//       if the column is not declared, we don't need to update it in the entities

			void IfExists(string propertyName, Action action)
			{
				if (model.DataRow.MetadataProperties.Any(mpd => mpd.Name == propertyName))
				{
					action();
				}
			}

			IfExists(nameof(model.FirstName), () => userEntity.FirstName = model.FirstName);
			IfExists(nameof(model.LastName), () => userEntity.LastName = model.LastName);
			IfExists(nameof(model.State), () => userEntity.State = model.State.Value);

			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;

			await repositorySession
				.Create<ApplicationUserEntityRepository>()
				.SaveOrUpdateAsync(userEntity);

			if (!String.IsNullOrEmpty(model.Password))
			{
				var changePasswordService = this.ServiceProvider.GetRequiredService<IChangePasswordCredentialsService>();
				await changePasswordService.ChangePasswordAsync(model.Email, model.Password);
			}

			await repositorySession.Session.FlushAsync();
		}
	}
}
