using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public class ProcessResult
	{
		private readonly List<ProcessResultContext> _contexts = new List<ProcessResultContext>();
		public ProcessResultContext[] Contexts => this._contexts.ToArray();
		public IProcessResultListener Listener { get; set; }

		public ProcessResultContext CreateContext(string name, string processName, object extendedProperties = null)
		{
			var context = new ProcessResultContext(name, processName, extendedProperties);
			this._contexts.Add(context);
			this.Listener?.OnResultContextAdded(context);
			context.EntryAdded += Context_EntryAdded;
			return context;
		}

		private void Context_EntryAdded(object sender, ProcessResultEntryAddedEventArgs e)
		{
			this.Listener?.OnResultEntryAdded(e.Entry, (ProcessResultContext)sender);
		}
	}
}
