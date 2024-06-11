using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public class SqlProcessResultEntry : ProcessResultEntry
	{
		public SqlProcessResultEntry(string content) : base("Sql", content)
		{
		}
	}
}
