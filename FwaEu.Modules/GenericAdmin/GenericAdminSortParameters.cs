using System.Collections.Generic;

namespace FwaEu.Modules.GenericAdmin
{
	public class GenericAdminSortParameter
	{
		public string ColumnName { get; }
		public bool Ascending { get; }

		public GenericAdminSortParameter(string columnName, bool ascending)
		{
			ColumnName = columnName;
			Ascending = ascending;
		}
	}

	public class GenericAdminSortParameters
	{
		public List<GenericAdminSortParameter> Parameters { get; }

		public GenericAdminSortParameters(List<GenericAdminSortParameter> parameters)
		{
			Parameters = parameters;
		}
	}
}
