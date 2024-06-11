using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportAdminApiModel
	{
		public Dictionary<string, string> Name { get; }
		public Dictionary<string, string> Description { get; }
		public string CategoryInvariantId { get; }
		public bool IsAsync { get; }
		public string Icon { get; }
		public ReportAdminNavigationApiModel Navigation { get; }
		public ReportAdminDataSourceApiModel DataSource { get; }
		public ReportAdminFilterApiModel[] Filters { get; }
		public ReportAdminPropertyApiModel[] Properties { get; }
		public Dictionary<string, ReportAdminViewApiModel[]> DefaultViews { get; }

		public ReportAdminApiModel(Dictionary<string, string> name,
				Dictionary<string, string> description, string categoryInvariantId, bool isAsync, string icon,
				ReportAdminNavigationApiModel navigation,
				ReportAdminDataSourceApiModel dataSource,
				ReportAdminFilterApiModel[] filters,
				ReportAdminPropertyApiModel[] properties,
				Dictionary<string, ReportAdminViewApiModel[]> defaultViews)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			CategoryInvariantId = categoryInvariantId;
			IsAsync = isAsync;
			Icon = icon;
			Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
			DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
			Filters = filters ?? throw new ArgumentNullException(nameof(filters));
			Properties = properties ?? throw new ArgumentNullException(nameof(properties));
			DefaultViews = defaultViews ?? throw new ArgumentNullException(nameof(defaultViews));
		}
	}

	public class ReportAdminFilterApiModel
	{
		public ReportAdminFilterApiModel(string invariantId, bool isRequired)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.IsRequired = isRequired;
		}

		public string InvariantId { get; set; }
		public bool IsRequired { get; set; }
	}

	public class ReportAdminPropertyApiModel
	{
		public ReportAdminPropertyApiModel(string name, string fieldInvariantId)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FieldInvariantId = fieldInvariantId;
		}

		public string Name { get; set; }
		public string FieldInvariantId { get; set; }
	}

	public class ReportAdminViewApiModel
	{
		public ReportAdminViewApiModel(bool isDefault, Dictionary<string, string> name, string value)
		{
			this.IsDefault = isDefault;
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Value = value;
		}
		public bool IsDefault { get; set; }
		public Dictionary<string, string> Name { get; set; }
		public string Value { get; set; }
	}

	public class ReportAdminNavigationApiModel
	{
		public ReportAdminNavigationApiModel(ReportAdminNavigationItemApiModel menu,
			ReportAdminNavigationItemApiModel summary)
		{
			this.Menu = menu ?? throw new ArgumentNullException(nameof(menu));
			this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
		}
		public ReportAdminNavigationItemApiModel Menu { get; }
		public ReportAdminNavigationItemApiModel Summary { get; }
	}

	public class ReportAdminNavigationItemApiModel
	{
		public ReportAdminNavigationItemApiModel(bool visible, int index)
		{
			this.Visible = visible;
			this.Index = index;
		}

		public bool Visible { get; }
		public int Index { get; }
	}

	public class ReportAdminDataSourceApiModel
	{
		public ReportAdminDataSourceApiModel(string type, string argument)
		{
			this.Type = type;
			this.Argument = argument;
		}

		public string Type { get; }
		public string Argument { get; }
	}

	public class ReportAdminSupportsSaveApiModel
	{
		public ReportAdminSupportsSaveApiModel(bool supportsSave)
		{
			this.SupportsSave = supportsSave;
		}

		public bool SupportsSave { get; }
	}

	public class ReportAdminLoadParameters
	{
		public string InvariantId { get; set; }
		public object Value { get; set; }
	}

	public class ReportAdminLoadDataSource : ReportAdminDataSourceApiModel
	{
		public ReportAdminLoadDataSource(string type, string argument, ReportAdminLoadParameters[] parameters)
			: base(type, argument)
		{
			this.Parameters = parameters;
		}
		public ReportAdminLoadParameters[] Parameters { get; set; }
	}
}

