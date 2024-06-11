using System;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportAdminListItemApiModel
	{
		public ReportAdminListItemApiModel(string name, string categoryInvariantId, string invariantId, string icon, bool supportSave)
		{
			Name = name;
			CategoryInvariantId = categoryInvariantId;
			InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			Icon = icon;
			SupportSave = supportSave;
		}

		public string Name { get; }
		public string CategoryInvariantId { get; }
		public string InvariantId { get; }
		public string Icon { get; }
		public bool SupportSave { get; }
	}
}
