using FwaEu.Modules.Reports;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsPerimeters
{
	public class EntityReportAccessService : IReportAccessService
	{
		private readonly IUserPerimeterService _userPerimeterService;

		public EntityReportAccessService(IUserPerimeterService userPerimeterService)
		{
			this._userPerimeterService = userPerimeterService
				?? throw new ArgumentNullException(nameof(userPerimeterService));
		}

		private async Task<UserPerimeterModel> GetPerimeterAsync(int userId)
		{
			return await this._userPerimeterService.GetAccessAsync(
				userId, ReportPerimeterProviderFactory.ProviderKey);
		}

		public async Task<ReportAccessResult> GetAccessAsync(int userId)
		{
			var perimeter = await this.GetPerimeterAsync(userId);

			return new ReportAccessResult(perimeter.HasFullAccess,
				perimeter.AccessibleIds == null ? null
				: perimeter.AccessibleIds.Cast<string>().ToArray());
		}

		public async Task<bool> IsAccessibleAsync(string reportInvariantId, int userId)
		{
			var perimeter = await this.GetPerimeterAsync(userId);
			return perimeter.HasFullAccess || perimeter.AccessibleIds.Cast<string>().Contains(reportInvariantId);
		}
	}
}
