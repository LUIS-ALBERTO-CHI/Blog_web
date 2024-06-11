using FwaEu.Fwamework.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Linq;

namespace FwaEu.Fwamework.Tests.Globalization
{
	[TestClass]
	public class LocalizableStringsOwnerTypeDescriptionProviderTests
	{
		private static FrenchEnglishCulturesServiceStub CulturesService;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			CulturesService = new FrenchEnglishCulturesServiceStub();

			var services = new ServiceCollection();
			services.AddSingleton<ICulturesService>(CulturesService);

			ApplicationServices.Initialize(services.BuildServiceProvider());
		}

		[TestMethod]
		public void GetProperties_Count()
		{
			var model = new ModelStub();

			var expectedFromTypeDescriptorPropertyNames = new[]
			{
				nameof(model.Id)
			};

			var removedByTypeDescriptorPropertyNames = new[]
			{
				nameof(model.InitializedProperty),
				nameof(model.NotInitializedProperty),
			};

			var typeDescriptorProperties = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.ToArray();

			Assert.AreEqual(expectedFromTypeDescriptorPropertyNames.Length +
				removedByTypeDescriptorPropertyNames.Length * CulturesService.AvailableCultures.Length,
				typeDescriptorProperties.Length);
		}

		[TestMethod]
		public void GetProperties_Naming()
		{
			var model = new ModelStub();

			var expectedFromTypeDescriptorPropertyNames = new[]
			{
				nameof(model.Id)
			};

			var removedByTypeDescriptorPropertyNames = new[]
			{
				nameof(model.InitializedProperty),
				nameof(model.NotInitializedProperty),
			};

			var expectedPropertyNames = expectedFromTypeDescriptorPropertyNames.Concat(
				removedByTypeDescriptorPropertyNames.SelectMany(propertyName =>
				CulturesService.AvailableCultures.Select(culture =>
					propertyName + culture.TwoLetterISOLanguageName.ToUpper())))
				.ToArray();

			var typeDescriptorProperties = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.ToArray();

			var matches = expectedPropertyNames.Join(typeDescriptorProperties,
				epn => epn, tdp => tdp.Name, (epn, tdp) => epn)
				.ToArray();

			Assert.AreEqual(typeDescriptorProperties.Length, matches.Length);
		}

		[TestMethod]
		public void GetProperties_GetValue()
		{
			var frenchTwoLetterIsoCode = CulturesService.FrenchCulture.TwoLetterISOLanguageName;
			var englishTwoLetterIsoCode = CulturesService.EnglishCulture.TwoLetterISOLanguageName;

			var model = new ModelStub();
			model.InitializedProperty[frenchTwoLetterIsoCode] = "Bonjour";

			var typeDescriptorPropertiesByName = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.ToDictionary(pd => pd.Name);

			object GetValue(string propertyName)
			{
				return typeDescriptorPropertiesByName[propertyName].GetValue(model);
			}

			Assert.AreEqual(model.Id, GetValue(nameof(model.Id)));
			Assert.AreEqual("Bonjour", GetValue(nameof(model.InitializedProperty) + frenchTwoLetterIsoCode.ToUpper()));

			Assert.IsNull(GetValue(nameof(model.InitializedProperty) + englishTwoLetterIsoCode.ToUpper()));
			Assert.IsNull(GetValue(nameof(model.NotInitializedProperty) + frenchTwoLetterIsoCode.ToUpper()));
			Assert.IsNull(GetValue(nameof(model.NotInitializedProperty) + englishTwoLetterIsoCode.ToUpper()));
		}

		[TestMethod]
		public void GetProperties_SetValue()
		{
			var frenchTwoLetterIsoCode = CulturesService.FrenchCulture.TwoLetterISOLanguageName;
			var englishTwoLetterIsoCode = CulturesService.EnglishCulture.TwoLetterISOLanguageName;

			var model = new ModelStub();
			model.NotInitializedProperty = null; // NOTE: To guarantee that SetValue() will create the LocalizableStringDictionary

			var typeDescriptorPropertiesByName = TypeDescriptor.GetProperties(model)
				.Cast<PropertyDescriptor>()
				.ToDictionary(pd => pd.Name);

			void SetValue(string propertyName, object value)
			{
				typeDescriptorPropertiesByName[propertyName].SetValue(model, value);
			}

			var originalId = model.Id;
			SetValue(nameof(model.Id), originalId + 1);
			Assert.AreEqual(originalId + 1, model.Id);

			SetValue(nameof(model.InitializedProperty) + frenchTwoLetterIsoCode.ToUpper(), "Bonjour");
			Assert.AreEqual("Bonjour", model.InitializedProperty[frenchTwoLetterIsoCode]);
			Assert.IsNull(model.InitializedProperty[englishTwoLetterIsoCode]);

			SetValue(nameof(model.NotInitializedProperty) + frenchTwoLetterIsoCode.ToUpper(), "Coucou");
			Assert.IsNotNull(model.NotInitializedProperty);
			Assert.IsInstanceOfType(model.NotInitializedProperty, typeof(LocalizableStringDictionary));
			Assert.AreEqual("Coucou", model.NotInitializedProperty[frenchTwoLetterIsoCode]);
			Assert.IsNull(model.NotInitializedProperty[englishTwoLetterIsoCode]);
		}
	}
}
