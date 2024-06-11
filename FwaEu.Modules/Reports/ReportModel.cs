using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportModel<TDisplay> : ReportModelBase<TDisplay>
	{
		public ReportModel(TDisplay name, TDisplay description,
			string categoryInvariantId, bool isAsync, string icon, ReportNavigationModel navigation, 
			ReportDataSource dataSource, ReportFilterModel[] filters,
			ReportPropertyModel[] properties,
			Dictionary<string, ReportViewModel<TDisplay>[]> defaultViews)
			: base(name, description, categoryInvariantId, isAsync, icon, navigation)
		{
			this.DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
			this.Filters = filters ?? throw new ArgumentNullException(nameof(filters));
			this.Properties = properties ?? throw new ArgumentNullException(nameof(properties));
			this.DefaultViews = defaultViews ?? throw new ArgumentNullException(nameof(defaultViews));
		}
		public ReportDataSource DataSource { get; }
		public ReportFilterModel[] Filters { get; }
		public ReportPropertyModel[] Properties { get; }
		public Dictionary<string, ReportViewModel<TDisplay>[]> DefaultViews { get; }
	}

	public class ReportDataSource
	{
		public ReportDataSource(string type, string argument) 
		{
			this.Type = type;
			this.Argument = argument;
		}

		public string Type { get; }
		public string Argument { get; }
	}

	public class ReportFilterModel
	{
		public ReportFilterModel(string invariantId, bool isRequired)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.IsRequired = isRequired;
		}

		public string InvariantId { get; }
		public bool IsRequired { get; }
	}

	public class ReportPropertyModel
	{
		public ReportPropertyModel(string name, string fieldInvariantId)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.FieldInvariantId = fieldInvariantId;
		}

		public string Name { get; }
		public string FieldInvariantId { get; }
	}

	public class ReportViewModel<TDisplay>
	{
		public ReportViewModel(bool isDefault, TDisplay name, string value)
		{
			this.IsDefault = isDefault;
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Value = value;
		}

		public bool IsDefault { get; }
		public TDisplay Name { get; }
		public string Value { get; }
	}
}
