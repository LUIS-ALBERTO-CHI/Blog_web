using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.Tests.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class SearchOnDataReaderStub : DataReaderStub<SearchOnTestModel>
	{
		public SearchOnDataReaderStub(SearchOnTestModel[] models) : base(models)
		{
			this._properties = new[]
			{
				new ModelPropertyDescriptor(nameof(SearchOnTestModel.Name), IsKeyValue.True, false, null, null),
				new ModelPropertyDescriptor(nameof(SearchOnTestModel.RelatedModel), IsKeyValue.False, false,
				new []{
						nameof(SearchOnTestModel.RelatedModel.Name),
						nameof(SearchOnTestModel.RelatedModel.Age),
				}, null),
			};
		}

		public override Dictionary<string, object> ToDictionary(SearchOnTestModel model)
		{
			var dictionary = new Dictionary<string, object>();

			dictionary.Add(nameof(SearchOnTestModel.Name), model.Name);
			dictionary.Add(nameof(SearchOnTestModel.RelatedModel),
				new object[] { model.RelatedModel.Name, model.RelatedModel.Age.ToString() });

			return dictionary;
		}
	}
}
