using System.Collections.Generic;

namespace FwaEu.Modules.GenericAdmin
{
	public enum GenericAdminFilterMode
	{
		None,
		And,
		Or,
		Contains,
		NotContains,
		StartsWith,
		EndsWith,
		Equals,
		NotEquals,
		GreaterThan,
		LessThan,
		GreaterOrEqual,
		LessOrEqual,
	}

	public class GenericAdminFilterCondition
	{
		public string ColumnName { get; set; }
		public GenericAdminFilterMode Mode { get; set; }
		public object Value { get; set; }
	}

	public class GenericAdminFilterConditionContainerValue
	{
		public GenericAdminFilterConditionContainer Container { get; set; }
		public GenericAdminFilterMode ContainerMode { get; set; }
	}

	public class GenericAdminFilterConditionContainer
	{
		public bool IsValue => Condition != null;
		public GenericAdminFilterCondition Condition { get; set; }
		public List<GenericAdminFilterConditionContainerValue> Container { get; set; }
	}

	public class GenericAdminFilterParameters
	{
		public GenericAdminFilterConditionContainer Filters { get; set; }
	}
}
