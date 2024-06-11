using System;
using System.Collections.Generic;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public class NhibernateConfigurationOptions
	{
		public string DatabaseFeaturesTypeFullName { get; set; }
		public Dictionary<string, string> Properties { get; set; }
	}

	public class SimpleNhibernateOptions
	{
		public bool LogFormattedSql { get; set; } = false;
		public bool LogSqlInConsole { get; set; } = false;
	}
}
