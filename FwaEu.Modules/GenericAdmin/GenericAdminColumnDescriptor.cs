namespace FwaEu.Modules.GenericAdmin
{
	public class GenericAdminColumnDescriptor
	{
		public string ColumnName { get; }
		public string Locale { get; }

		public GenericAdminColumnDescriptor(string columnName)
		{
			var parts = columnName.Split('.');
			ColumnName = parts[0];
			if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
				Locale = parts[1];
		}
	}
}
