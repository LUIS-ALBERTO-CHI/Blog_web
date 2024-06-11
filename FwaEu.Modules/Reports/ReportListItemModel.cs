using System;

namespace FwaEu.Modules.Reports
{
	public class ReportListItemModel : ReportModelBase<string>
	{
		public ReportListItemModel(
			string invariantId, bool hasFilters,
			string name, string description,
			string categoryInvariantId, ReportNavigationModel navigation,
			bool isAsync, string icon)
			: base(name, description, categoryInvariantId, isAsync, icon, navigation)
		{
			this.HasFilters = hasFilters;
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
		}

		public bool HasFilters { get; }
		public string InvariantId { get; }
	}
}
