using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class PropertyBinderTestModel
	{
		public string PropertyWithPublicSetter { get; set; }
		public string PropertyWithProtectedSetter { get; protected set; }
	}
}
