using FwaEu.Fwamework.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Text
{
	[TestClass]
	public class PluralizeNetCorePluralizationServiceTests
	{
		public static IEnumerable<object[]> GetWords()
		{
			// NOTE: Could be upgrade using a full dataset, like https://en.islcollective.com/english-esl-worksheets/grammar/nouns/plural-nouns-conversion-rules/120750

			yield return new object[] { "romain", "romains" };
			yield return new object[] { "rule", "rules" };

			// y => ies
			yield return new object[] { "city", "cities" };

			// s -> sses
			yield return new object[] { "rule", "rules" };

			// o -> os
			yield return new object[] { "zoo", "zoos" };

			// o -> oes
			yield return new object[] { "tomato", "tomatoes" };

			// Irregular
			yield return new object[] { "ox", "oxen" };
			yield return new object[] { "trout", "trout" };
			yield return new object[] { "focus", "foci" };
			yield return new object[] { "quiz", "quizzes" };
			yield return new object[] { "index", "indices" };
			yield return new object[] { "datum", "data" };
			yield return new object[] { "genus", "genera" };
			yield return new object[] { "leaf", "leaves" };
			yield return new object[] { "woman", "women" };
			yield return new object[] { "child", "children" };
		}

		[TestMethod]
		[DynamicData(nameof(GetWords), DynamicDataSourceType.Method)]
		public void Pluralize(string singular, string pluralExpected)
		{
			var service = new PluralizeNetCorePluralizationService();
			var plural = service.Pluralize(singular);

			Assert.AreEqual(pluralExpected, plural,
				$"The word '{singular}' should be converted to '{pluralExpected}', " +
				$"but was converted to '{plural}'.");
		}

		[TestMethod]
		[DynamicData(nameof(GetWords), DynamicDataSourceType.Method)]
		public void Singularize(string singularExpected, string plural)
		{
			var service = new PluralizeNetCorePluralizationService();
			var singular = service.Singularize(plural);

			Assert.AreEqual(singularExpected, singular,
				$"The word '{plural}' should be converted to '{singularExpected}', " +
				$"but was converted to '{singular}'.");
		}
	}
}
