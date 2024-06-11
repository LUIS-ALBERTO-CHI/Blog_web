using FwaEu.Fwamework.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FwaEu.Fwamework.Tests.WebAPI
{
	[TestClass]
	public class DefaultModelValidationServiceTests
	{
		[TestMethod]
		public void Save()
		{
			var service = new DefaultModelValidationService();

			var goodModel = new CatModel()
			{
				Name = "Felix",
				Age = 42
			};

			var badModel = new CatModel()
			{
				Name = "Isidore",
				Age = null
			};


			var goodModelErrors = service.Validate(goodModel);
			Assert.AreEqual(0, goodModelErrors.Count());

			var badModelErrors = service.Validate(badModel);
			Assert.AreNotEqual(0, badModelErrors.Count());
		}
	}

	public class CatModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public int? Age { get; set; }
	}
}
