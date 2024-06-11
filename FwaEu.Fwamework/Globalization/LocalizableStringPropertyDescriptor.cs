using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public class LocalizableStringPropertyDescriptor : PropertyDescriptor
	{
		public LocalizableStringPropertyDescriptor(PropertyDescriptor ownerProperty, string cultureTwoLetterIsoCode)
			: base(ownerProperty.Name + cultureTwoLetterIsoCode.ToUpper(), null)
		{
			this.OwnerProperty = ownerProperty;
			this.CultureTwoLetterIsoCode = cultureTwoLetterIsoCode;
		}

		public PropertyDescriptor OwnerProperty { get; private set; }
		protected string CultureTwoLetterIsoCode { get; private set; }

		public override bool CanResetValue(object component) { return true; }
		public override Type ComponentType { get { return typeof(string); } }

		public override object GetValue(object component)
		{
			var localizableString = (IDictionary)this.OwnerProperty.GetValue(component);

			if (localizableString == null)
			{
				return null;
			}

			return localizableString[this.CultureTwoLetterIsoCode];
		}

		public override bool IsReadOnly { get { return false; } }
		public override Type PropertyType { get { return typeof(string); } }

		public override void ResetValue(object component)
		{
			this.SetValue(component, default(string));
		}

		public override void SetValue(object component, object value)
		{
			var localizableString = (IDictionary)this.OwnerProperty.GetValue(component);
			if (localizableString == null)
			{
				this.OwnerProperty.SetValue(component,
					localizableString = new LocalizableStringDictionary());
			}

			localizableString[this.CultureTwoLetterIsoCode] = (string)value;
		}

		public override bool ShouldSerializeValue(object component) { return true; }
	}

}
