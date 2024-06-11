using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.MasterData
{
	public class EntityPermissionMasterDataDate : IPermissionMasterDataDate
	{
		private readonly MainSessionContext _sessionContext;

		public EntityPermissionMasterDataDate(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<DateTime?> GetMaximumUpdatedOnAsync()
		{
			return await this._sessionContext.RepositorySession
				.Create<PermissionEntityRepository>()
				.Query()
				.MaxAsync(p => (DateTime?)p.UpdatedOn);
		}
	}
}
