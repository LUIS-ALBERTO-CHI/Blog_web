using FwaEu.Modules.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Reports
{
	[TestClass]
	public class ReportModelLocalizableStringTests
	{
		private static ReportLocalizableString CreateReportLocalizableString(string en, string fr)
		{
			return new ReportLocalizableString()
			{
				{ "en", en },
				{ "fr", fr },
			};
		}

		private static ReportModel<ReportLocalizableString> CreateModelStub()
		{
			var navigation = new ReportNavigationModel(
			   new ReportNavigationItemModel(true, 0),
			   new ReportNavigationItemModel(true, 0));

			bool isAsync = false;

			var dataSource = new ReportDataSource("MasterData", "Users");

			var report = new ReportModel<ReportLocalizableString>(CreateReportLocalizableString("ReportEn", "ReportFr"),
				CreateReportLocalizableString("DescriptionEn", "DescriptionFr"), "CategoryId",
				isAsync, null, navigation, dataSource,
				new ReportFilterModel[] { }, new ReportPropertyModel[] { },
				new Dictionary<string, ReportViewModel<ReportLocalizableString>[]>());

			return report;
		}

		private const string JsonModelStub =
@"{
  ""DataSource"": {
    ""Type"": ""MasterData"",
    ""Argument"": ""Users""
  },
  ""Filters"": [],
  ""Properties"": [],
  ""DefaultViews"": {},
  ""Name"": {
    ""en"": ""ReportEn"",
    ""fr"": ""ReportFr""
  },
  ""Description"": {
    ""en"": ""DescriptionEn"",
    ""fr"": ""DescriptionFr""
  },
  ""CategoryInvariantId"": ""CategoryId"",
  ""Navigation"": {
    ""Menu"": {
      ""Visible"": true,
      ""Index"": 0
    },
    ""Summary"": {
      ""Visible"": true,
      ""Index"": 0
    }
  },
  ""IsAsync"": false,
  ""Icon"": null
}";

		[TestMethod]
		public void JsonSerialize()
		{
			var modelStub = CreateModelStub();
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(modelStub,
				Newtonsoft.Json.Formatting.Indented);

			Assert.AreEqual(JsonModelStub, json);
		}

		[TestMethod]
		public void JsonDeserialize()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<
				ReportModel<ReportLocalizableString>>(JsonModelStub);

			var expectedModel = CreateModelStub();

			Assert.AreEqual(expectedModel.Name["en"], model.Name["en"]);
		}
	}
}
