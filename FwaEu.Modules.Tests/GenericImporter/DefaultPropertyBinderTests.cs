using FwaEu.Modules.GenericImporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	[TestClass]
	public class DefaultPropertyBinderTests
	{
		[TestMethod]
		public void SetValue_PropertyWithPublicSetter()
		{
			var binder = new DefaultPropertyBinder();
			var model = new PropertyBinderTestModel();

			var property = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.Where(p => p.Name == nameof(model.PropertyWithPublicSetter))
				.First();

			model.PropertyWithPublicSetter = null;
			Assert.IsNull(model.PropertyWithPublicSetter);

			binder.SetValue(property, model, "NewValue");
			Assert.AreEqual("NewValue", model.PropertyWithPublicSetter);
		}

		[TestMethod]
		public void SetValue_PropertyWithProtectedSetter()
		{
			var binder = new DefaultPropertyBinder();
			var model = new PropertyBinderTestModel();

			var property = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.Where(p => p.Name == nameof(model.PropertyWithProtectedSetter))
				.First();

			Assert.IsNull(model.PropertyWithProtectedSetter);

			binder.SetValue(property, model, "NewValue");
			Assert.AreEqual("NewValue", model.PropertyWithProtectedSetter);
		}
	}
}
