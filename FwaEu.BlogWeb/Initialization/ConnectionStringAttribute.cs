using System;

namespace FwaEu.BlogWeb.Initialization
{
	public class ConnectionStringAttribute : Attribute
	{
		public ConnectionStringAttribute(string name)
		{
			this.Name = name;
		}

		public string Name { get; }
	}
}
