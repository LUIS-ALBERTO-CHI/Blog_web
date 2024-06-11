using FwaEu.Fwamework.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FwaEu.Fwamework.Tests.Globalization
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<ModelStub>))]
	public class ModelStub
	{
		public int Id { get; set; } = 1;

		[LocalizableString]
		public IDictionary InitializedProperty { get; set; } = new LocalizableStringDictionary();

		[LocalizableString]
		public IDictionary NotInitializedProperty { get; set; }
	}
}
