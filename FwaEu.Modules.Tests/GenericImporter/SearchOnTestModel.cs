using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class SearchOnTestModel
	{
		public string Name { get; set; }
		public SearchOnRelatedTestModel RelatedModel { get; set; }
	}

	public class SearchOnRelatedTestModel
	{
		public string Name { get; set; }
		public int Age { get; set; }
	}
}
