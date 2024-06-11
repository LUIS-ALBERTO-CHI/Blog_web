using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.MasterData
{
	public interface IPermissionMasterDataDate
	{
		Task<DateTime?> GetMaximumUpdatedOnAsync();
	}
}
