using Aspose.Cells;
using FwaEu.Modules.GenericImporterWorksheet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FwaEu.Modules.GenericImporter;

namespace FwaEu.Modules.Tests.GenericImporterWorksheet
{
	[TestClass]
	public class IncludingSchemaWorksheetDataReaderTests
	{
		private static Lazy<Workbook> Workbook = new Lazy<Workbook>(
			() => new Workbook(Path.GetFullPath("GenericImporterWorksheet/Worksheet.xlsx")));

		private static Worksheet GetPropertiesWorksheet()
		{
			return Workbook.Value.Worksheets["Properties"];
		}

		private static Worksheet GetDataWorksheet()
		{
			return Workbook.Value.Worksheets["Data"];
		}

		private static WorksheetModelPropertyDescriptor[] GetProperties()
		{
			var worksheet = GetPropertiesWorksheet();

			return new IncludingSchemaWorksheetDataReader(
				SchemaDescriptor.Default, worksheet).GetProperties()
				.Cast<WorksheetModelPropertyDescriptor>()
				.ToArray();
		}

		[TestMethod]
		public void LoadProperties_IsKey()
		{
			var properties = GetProperties();

			Assert.AreEqual(IsKeyValue.True, properties.First(p => p.Name == "Property1").IsKey);
			Assert.AreEqual(IsKeyValue.TrueAllowNull, properties.First(p => p.Name == "Property2").IsKey);
			Assert.AreEqual(IsKeyValue.False, properties.First(p => p.Name == "Property3").IsKey);
			Assert.AreEqual(IsKeyValue.False, properties.First(p => p.Name == "Property4").IsKey);
		}

		[TestMethod]
		public void LoadProperties_SearchOn()
		{
			var properties = GetProperties();

			Assert.IsNull(properties.First(p => p.Name == "Property1").SearchOn);
			Assert.IsNull(properties.First(p => p.Name == "Property2").SearchOn);

			var property3SearchOn = properties.First(p => p.Name == "Property3");
			Assert.IsNotNull(property3SearchOn);
			Assert.AreEqual(2, property3SearchOn.SearchOn.Length);
			Assert.AreEqual(0, property3SearchOn.SearchOn.Except(new[] { "A", "B" }).Count());

			var property4SearchOn = properties.First(p => p.Name == "Property4");
			Assert.IsNotNull(property4SearchOn);
			Assert.AreEqual(2, property4SearchOn.SearchOn.Length);
			Assert.AreEqual(0, property4SearchOn.SearchOn.Except(new[] { "C", "D" }).Count());
		}

		[TestMethod]
		public void LoadProperties_DisplayName()
		{
			var properties = GetProperties();

			Assert.AreEqual("Property 1 Display Name",
				properties.First(p => p.Name == "Property1").DisplayName);

			Assert.AreEqual("Property 2 Display Name",
				properties.First(p => p.Name == "Property2").DisplayName);

			Assert.AreEqual("Property3",
				properties.First(p => p.Name == "Property3").DisplayName,
				"Empty in Excel file should result as display name == name");

			Assert.AreEqual("Property4",
				properties.First(p => p.Name == "Property4").DisplayName,
				"Empty in Excel file should result as display name == name");
		}

		[TestMethod]
		public void GetRows_TypesAndNull()
		{
			var dataReader = new IncludingSchemaWorksheetDataReader(
				SchemaDescriptor.Default, GetDataWorksheet());

			var rows = dataReader.GetRows().ToArray();
			Assert.AreEqual(1, rows.Length);

			var row = rows[0];
			Assert.AreEqual("String", row.ValuesByPropertyName["Property1"]);
			Assert.AreEqual(10.0, row.ValuesByPropertyName["Property2"]);
			Assert.AreEqual(new DateTime(2019, 10, 10, 0, 0, 0), row.ValuesByPropertyName["Property3"]);
			Assert.AreEqual(true, row.ValuesByPropertyName["Property4"]);
			Assert.AreEqual(null, row.ValuesByPropertyName["Property5"]);
		}
	}
}
