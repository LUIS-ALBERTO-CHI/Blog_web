using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public interface IModelValidationService
	{
		string[] Validate(object typedArguments);
	}
}
