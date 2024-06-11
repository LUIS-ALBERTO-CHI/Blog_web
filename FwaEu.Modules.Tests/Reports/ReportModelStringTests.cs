using FwaEu.Modules.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Reports
{
	[TestClass]
	public class ReportModelStringTests
	{
		private static JsonServiceReportModel CreateModelStub()
		{
			var navigation = new ReportNavigationModel(
			   new ReportNavigationItemModel(true, 0),
			   new ReportNavigationItemModel(true, 0));

			var dataSource = new ReportDataSource("MasterData", "Users");

			var report = new JsonServiceReportModel("Report", "Description", "CategoryId", false,
				navigation, dataSource,
				new ReportFilterModel[] { }, new ReportPropertyModel[] { },
				new Dictionary<string, ReportViewModel<string>[]>());

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
  ""Name"": ""Report"",
  ""Description"": ""Description"",
  ""CategoryInvariantId"": ""CategoryId"",
  ""IsAsync"": false,
  ""Icon"": null,
  ""Navigation"": {
    ""Menu"": {
      ""Visible"": true,
      ""Index"": 0
    },
    ""Summary"": {
      ""Visible"": true,
      ""Index"": 0
    }
  }
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
			var jsonModel = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonServiceReportModel>(JsonModelStub);
			var model = new ServiceReportModel("StubReport", jsonModel.Name, jsonModel.Description,
				jsonModel.CategoryInvariantId, jsonModel.IsAsync, jsonModel.Icon, jsonModel.Navigation, jsonModel.DataSource, jsonModel.Filters,
				jsonModel.Properties, jsonModel.DefaultViews);
			var expectedModel = CreateModelStub();

			Assert.AreEqual(expectedModel.Name, model.Name);
		}
	}
}
