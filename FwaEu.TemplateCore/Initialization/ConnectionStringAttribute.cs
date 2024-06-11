using System;

namespace FwaEu.TemplateCore.Initialization
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
