using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public class DefaultModelValidationService : IModelValidationService
	{
		public string[] Validate(object model)
		{
			ValidationContext context = new ValidationContext(model);
			ICollection<ValidationResult> results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);

			return results.Select(x => x.ErrorMessage).ToArray();
		}
	}
}
