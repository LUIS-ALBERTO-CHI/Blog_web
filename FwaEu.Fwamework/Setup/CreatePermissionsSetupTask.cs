using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class CreatePermissionsSetupTask : ISetupTask
	{
		public CreatePermissionsSetupTask(
			ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory,
			IPermissionProviderRegister permissionProviderRegister)
		{
			this._sessionAdapterFactory = sessionAdapterFactory;
			this._repositoryFactory = repositoryFactory;
			this._permissionProviderRegister = permissionProviderRegister;
		}

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly IPermissionProviderRegister _permissionProviderRegister;

		public string Name => "CreatePermissions";
		public Type ArgumentsType => null;

		private static void AddInfo(ProcessResultContext context, string format, IEnumerable<IPermission> permissions)
		{
			context.Add(new InfoProcessResultEntry(
				String.Format(format, String.Join(", ", permissions.Select(p => p.InvariantId)))));
		}

		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var result = new ProcessResult();
			var permissionProviders = this._permissionProviderRegister.GetProviders().ToArray();

			if (permissionProviders.Any())
			{
				using (var session = this._sessionAdapterFactory.CreateStatefulSession())
				{
					var permissionRepository = this._repositoryFactory.Create<PermissionEntityRepository>(session);
					var existingPermissions = permissionRepository.Query().Select(p => new { p.InvariantId }).ToArray();

					foreach (var provider in permissionProviders)
					{
						var context = result.CreateContext($"CreatePermissions from {provider.GetType().Name}", "CreatePermissionsSetupTask");
						var permissions = provider.GetPermissions().ToArray();

						var existing = permissions.Join(existingPermissions, p => p.InvariantId, ep => ep.InvariantId, (p, ep) => p).ToArray();
						var toCreate = permissions.Except(existing).ToArray();

						foreach (var @new in toCreate)
						{
							await permissionRepository.SaveOrUpdateAsync(new PermissionEntity(@new.InvariantId));
						}

						await session.FlushAsync();

						if (toCreate.Any())
						{
							AddInfo(context, "Permissions created: {0}", toCreate);
						}

						if (existing.Any())
						{
							AddInfo(context, "Permissions already existing: {0}", existing);
						}
					}
				}
			}
			else
			{
				result.CreateContext("Create permissions", "CreatePermissionsSetupTask")
					.Add(new InfoProcessResultEntry("No permissions exists in current application"));
			}

			return new NoDataSetupTaskResult(result);
		}
	}
}
