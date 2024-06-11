using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportApiModelBase
	{
		protected ReportApiModelBase(string name, string description,
			string categoryInvariantId, ReportNavigationApiModel navigation, 
			bool isAsync, string icon)
		{
			this.Name = name
				?? throw new ArgumentNullException(nameof(name));

			this.CategoryInvariantId = categoryInvariantId;

			this.Navigation = navigation
				?? throw new ArgumentNullException(nameof(navigation));

			this.Description = description;
			this.IsAsync = isAsync;
			this.Icon = icon;
		}

		public string Name { get; }
		public string Description { get; }
		public string CategoryInvariantId { get; }
		public ReportNavigationApiModel Navigation { get; }
		public bool IsAsync { get; }
		public string Icon { get; }
	}

	public class ReportNavigationApiModel
	{
		public ReportNavigationApiModel(ReportNavigationItemApiModel menu, ReportNavigationItemApiModel summary)
		{
			this.Menu = menu ?? throw new ArgumentNullException(nameof(menu));
			this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
		}

		public ReportNavigationItemApiModel Menu { get; }
		public ReportNavigationItemApiModel Summary { get; }
	}

	public class ReportNavigationItemApiModel
	{
		public ReportNavigationItemApiModel(bool visible, int index)
		{
			this.Visible = visible;
			this.Index = index;
		}

		public bool Visible { get; }
		public int Index { get; }
	}
}
