using FwaEu.Modules.Reports.WebApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ServiceReportModel : ReportModel<string>
	{
		public ServiceReportModel(string invariantId, string name, string description, string categoryInvariantId, bool isAsync, string icon,
			ReportNavigationModel navigation, ReportDataSource dataSource, ReportFilterModel[] filters,
			ReportPropertyModel[] properties, Dictionary<string, ReportViewModel<string>[]> defaultViews)
			: base(name, description, categoryInvariantId, isAsync, icon,
				  navigation, dataSource, filters,
				  properties, defaultViews)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
		}
		public string InvariantId { get; }

	}

	public class JsonServiceReportModel
	{
		public JsonServiceReportModel(string name, string description, string categoryInvariantId, bool isAsync,
			ReportNavigationModel navigation, ReportDataSource dataSource, ReportFilterModel[] filters,
			ReportPropertyModel[] properties, Dictionary<string, ReportViewModel<string>[]> defaultViews)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Description = description;
			this.CategoryInvariantId = categoryInvariantId;
			this.IsAsync = isAsync;
			this.Navigation = navigation
				?? throw new ArgumentNullException(nameof(navigation));
			this.DataSource = dataSource
				?? throw new ArgumentNullException(nameof(dataSource));
			this.Filters = filters
				?? throw new ArgumentNullException(nameof(filters));
			this.Properties = properties
				?? throw new ArgumentNullException(nameof(properties));
			this.DefaultViews = defaultViews
				?? throw new ArgumentNullException(nameof(defaultViews));
		}

		public string Name { get; }
		public string Description { get; }
		public string CategoryInvariantId { get; }
		public bool IsAsync { get; }
		public string Icon { get; }
		public ReportNavigationModel Navigation { get; }
		public ReportDataSource DataSource;
		public ReportFilterModel[] Filters;
		public ReportPropertyModel[] Properties;
		public Dictionary<string, ReportViewModel<string>[]> DefaultViews;
	}
}
