using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportAdminModel
	{
		public Dictionary<string, string> Name { get; }
		public Dictionary<string, string> Description { get; }
		public string CategoryInvariantId { get; }
		public ReportAdminNavigationModel Navigation { get; }
		public ReportAdminDataSourceModel DataSource { get; }
		public ReportAdminFilterModel[] Filters { get; }
		public ReportAdminPropertyModel[] Properties { get; }
		public Dictionary<string, ReportAdminViewModel[]> DefaultViews { get; }
		public bool IsAsync { get; }
		public string Icon { get; }
		public ReportAdminModel(Dictionary<string, string> name,
			Dictionary<string, string> description, string categoryInvariantId, bool isAsync, string icon,
			ReportAdminNavigationModel navigation,
			ReportAdminDataSourceModel dataSource,
			ReportAdminFilterModel[] filters,
			ReportAdminPropertyModel[] properties,
			Dictionary<string, ReportAdminViewModel[]> defaultViews)
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

	public class ReportAdminFilterModel
	{
		public ReportAdminFilterModel(string invariantId, bool isRequired)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.IsRequired = isRequired;			
		}

		public string InvariantId { get; set; }
		public bool IsRequired { get; set; }
	}

	public class ReportAdminPropertyModel
	{
		public ReportAdminPropertyModel(string name, string fieldInvariantId)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FieldInvariantId = fieldInvariantId;
		}

		public string Name { get; set; }
		public string FieldInvariantId { get; set; }
	}

	public class ReportAdminViewModel
	{
		public ReportAdminViewModel(bool isDefault, Dictionary<string, string> name, string value)
		{
			this.IsDefault = isDefault;
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Value = value;
		}
		public bool IsDefault { get; set; }
		public Dictionary<string, string> Name { get; set; }
		public string Value { get; set; }
	}

	public class ReportAdminNavigationModel
	{
		public ReportAdminNavigationModel(ReportAdminNavigationItemModel menu, ReportAdminNavigationItemModel summary)
		{
			this.Menu = menu ?? throw new ArgumentNullException(nameof(menu));
			this.Summary = summary ?? throw new ArgumentNullException(nameof(summary));
		}
		public ReportAdminNavigationItemModel Menu { get; }
		public ReportAdminNavigationItemModel Summary { get; }
	}

	public class ReportAdminNavigationItemModel
	{
		public ReportAdminNavigationItemModel(bool visible, int index)
		{
			this.Visible = visible;
			this.Index = index;
		}

		public bool Visible { get; }
		public int Index { get; }
	}

	public class ReportAdminDataSourceModel
	{
		public ReportAdminDataSourceModel(string type, string argument)
		{
			this.Type = type;
			this.Argument = argument;
		}

		public string Type { get; }
		public string Argument { get; }
	}
}
