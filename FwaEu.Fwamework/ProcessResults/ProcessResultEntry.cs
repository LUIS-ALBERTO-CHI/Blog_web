using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public abstract class ProcessResultEntry
	{
		protected ProcessResultEntry(string type, string content, string[] details = null)
		{
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
			this.Content = content ?? throw new ArgumentNullException(nameof(content));
			this.Details = details;
		}

		public string Type { get; }
		public string Content { get; }
		public string[] Details { get; }
	}
}
