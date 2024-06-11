using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Logging
{
	public interface ILogEnricherProvider
	{
		IEnumerable<LogEnricherProperty> GetProperties();
	}

	public class LogEnricherProperty
	{
		public LogEnricherProperty(string name, object value, bool destructure)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));

			this.Value = value;
			this.Destructure = destructure;
		}

		public string Name { get; }
		public object Value { get; }
		public bool Destructure { get; }
	}
}
