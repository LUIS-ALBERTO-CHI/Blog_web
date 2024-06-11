using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportAccessService
	{
		Task<bool> IsAccessibleAsync(string reportInvariantId, int userId);
		Task<ReportAccessResult> GetAccessAsync(int userId);
	}

	public class ReportAccessResult
	{
		public ReportAccessResult(bool hasFullAccess, string[] accessibleReportInvariantIds)
		{
			if (hasFullAccess && accessibleReportInvariantIds != null)
			{
				throw new ArgumentException(
					$"Must be null when {nameof(hasFullAccess)} is true.",
					nameof(accessibleReportInvariantIds));
			}

			if (!hasFullAccess && accessibleReportInvariantIds == null)
			{
				throw new ArgumentNullException(nameof(accessibleReportInvariantIds));
			}

			this.HasFullAccess = hasFullAccess;
			this.AccessibleReportInvariantIds = accessibleReportInvariantIds;
		}

		public bool HasFullAccess { get; }
		public string[] AccessibleReportInvariantIds { get; }
	}
}
