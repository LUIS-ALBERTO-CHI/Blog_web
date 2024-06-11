using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FwaEu.Fwamework.WebApi
{
	public class RequiredIfNullAttribute : RequiredAttribute
	{
		private string DependingPropertyName { get; set; }

		public RequiredIfNullAttribute(string dependingPropertyName)
		{
			this.DependingPropertyName = dependingPropertyName;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dependingPropertyValue = validationContext.ObjectInstance.GetType().GetProperty(DependingPropertyName).GetValue(validationContext.ObjectInstance, null);

			if (dependingPropertyValue == null && value == null)
			{
				return new ValidationResult(String.Format("Property must have a value if {0} is null.", this.DependingPropertyName));
			}
			else if (value != null && dependingPropertyValue != null)
			{
				return new ValidationResult(String.Format("Property cannot have a value if {0} is not null.", this.DependingPropertyName));
			}
			return ValidationResult.Success;
		}
	}
}
