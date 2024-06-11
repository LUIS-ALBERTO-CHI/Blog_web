using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportApiModel : ReportApiModelBase
	{
		public ReportApiModel(string name, string description, string categoryInvariantId, ReportNavigationApiModel navigation, bool isAsync, string icon,
			ReportDataSourceApiModel dataSource, ReportFilterApiModel[] filters, ReportPropertyApiModel[] properties, Dictionary<string, ReportViewApiModel[]> defaultViews) :
			base(name, description, categoryInvariantId, navigation, isAsync, icon)
		{
			this.DataSource = dataSource;
			this.Filters = filters;
			this.Properties = properties;
			this.DefaultViews = defaultViews;
		}

		public ReportDataSourceApiModel DataSource { get; }
		public ReportFilterApiModel[] Filters { get; }
		public ReportPropertyApiModel[] Properties { get; }
		public Dictionary<string, ReportViewApiModel[]> DefaultViews { get; }
	}

	public class ReportFilterApiModel
	{
		public ReportFilterApiModel(string invariantId, bool isRequired)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.IsRequired = isRequired;
		}

		public string InvariantId { get; set; }
		public bool IsRequired { get; set; }
	}

	public class ReportPropertyApiModel
	{
		public ReportPropertyApiModel(string name, string fieldInvariantId)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FieldInvariantId = fieldInvariantId;
		}

		public string Name { get; set; }
		public string FieldInvariantId { get; set; }
	}

	public class ReportViewApiModel
	{
		public ReportViewApiModel(bool isDefault, string name, string value)
		{
			this.IsDefault = isDefault;
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Value = value;
		}

		public bool IsDefault { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
	public class ReportDataSourceApiModel
	{
		public ReportDataSourceApiModel(string type, string argument)
		{
			this.Type = type;
			this.Argument = argument;
		}

		public string Type { get; }
		public string Argument { get; }
	}
}
