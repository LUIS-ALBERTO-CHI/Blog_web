using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public class ProcessResultContext
	{
		public ProcessResultContext(string name, string processName, object extendedProperties)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.ProcessName = processName ?? throw new ArgumentNullException(nameof(processName));
			this.ExtendedProperties = extendedProperties;
		}

		private readonly List<ProcessResultEntry> _entries = new List<ProcessResultEntry>();

		public string Name { get; }
		public string ProcessName { get; }
		public object ExtendedProperties { get; }

		public ProcessResultEntry[] Entries => this._entries.ToArray();

		public void Add(ProcessResultEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException(nameof(entry));
			}

			this._entries.Add(entry);
			this.EntryAdded?.Invoke(this, new ProcessResultEntryAddedEventArgs(entry));
		}

		public void Add(IEnumerable<ProcessResultEntry> entries)
		{
			foreach (var entry in entries)
			{
				this.Add(entry);
			}
		}

		public event EventHandler<ProcessResultEntryAddedEventArgs> EntryAdded;
	}

	public class ProcessResultEntryAddedEventArgs : EventArgs
	{
		public ProcessResultEntryAddedEventArgs(ProcessResultEntry entry)
		{
			this.Entry = entry ?? throw new ArgumentNullException(nameof(entry));
		}

		public ProcessResultEntry Entry { get; }
	}
}
