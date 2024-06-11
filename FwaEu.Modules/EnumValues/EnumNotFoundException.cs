using System;

namespace FwaEu.Modules.EnumValues
{
	public class EnumNotFoundException : Exception
	{
		public EnumNotFoundException(string message) : base(message)
		{
		}
	}
}
