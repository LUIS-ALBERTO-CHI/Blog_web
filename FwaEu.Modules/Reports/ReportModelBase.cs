using System;

namespace FwaEu.Modules.Reports
{
	public abstract class ReportModelBase<TDisplay>
	{
		protected ReportModelBase(TDisplay name, TDisplay description,
			string categoryInvariantId, bool isAsync, string icon, ReportNavigationModel navigation)
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

		public TDisplay Name { get; }
		public TDisplay Description { get; }
		public string CategoryInvariantId { get; }
		public ReportNavigationModel Navigation { get; }
		public bool IsAsync { get; }
		public string Icon { get; }
	}

	public class ReportNavigationModel
	{
		public ReportNavigationModel(ReportNavigationItemModel menu, ReportNavigationItemModel summary)
		{
			this.Menu = menu ?? throw new ArgumentNullException(nameof(menu));
			this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
		}

		public ReportNavigationItemModel Menu { get; }
		public ReportNavigationItemModel Summary { get; }
	}

	public class ReportNavigationItemModel
	{
		public ReportNavigationItemModel(bool visible, int index)
		{
			this.Visible = visible;
			this.Index = index;
		}

		public bool Visible { get; }
		public int Index { get; }
	}
}
