using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public class ModelPropertyDescriptor
	{
		public ModelPropertyDescriptor(string name, IsKeyValue isKey,
			bool isInfo, string[] searchOn, string displayName)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.IsKey = isKey;
			this.IsInfo = isInfo;
			this.SearchOn = searchOn;
			this.DisplayName = displayName ?? name;
		}

		public string Name { get; private set; }
		public IsKeyValue IsKey { get; private set; }
		public bool IsInfo { get; private set; }
		public string[] SearchOn { get; private set; }
		public string DisplayName { get; private set; }
	}
}
