using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportListItemApiModel : ReportApiModelBase
	{
		public ReportListItemApiModel(string invariantId, bool hasFilters,
			string name, string description,
			string categoryInvariantId, ReportNavigationApiModel navigation, 
			bool isAsync, string icon)
			: base(name, description, categoryInvariantId, navigation, isAsync, icon)
		{
			this.InvariantId = invariantId
				?? throw new ArgumentNullException(nameof(invariantId));
			this.HasFilters = hasFilters;
		}

		public string InvariantId { get; }

		public bool HasFilters { get; }
	}
}
