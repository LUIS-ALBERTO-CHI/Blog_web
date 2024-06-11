using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Swagger.Filters
{
	/// <summary>
	/// https://github.com/fooberichu150/swagger-optional-route-parameters/blob/master/Swagger.OptionalRouteParameters/Filters/ReApplyOptionalRouteParameterOperationFilter.cs
	/// </summary>
	public class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
	{
		const string captureName = "routeParameter";
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var httpMethodAttributes = context.MethodInfo
				.GetCustomAttributes(true)
				.OfType<HttpMethodAttribute>();
			//NOTE: m.Template != null added to original github code
			var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template != null && m.Template.Contains("?"));
			if (httpMethodWithOptional == null)
				return;
			string regex = $"{{(?<{captureName}>\\w+)\\?}}";
			var matches = Regex.Matches(httpMethodWithOptional.Template, regex);
			foreach (Match match in matches)
			{
				var name = match.Groups[captureName].Value;
				var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
				if (parameter != null)
				{
					parameter.AllowEmptyValue = true;
					parameter.Description = "Must check \"Send empty value\" or Swagger passes a comma for empty values otherwise";
					parameter.Required = false;
					//parameter.Schema.Default = new OpenApiString(string.Empty);

					parameter.Schema.Nullable = true;
				}
			}
		}
	}
}
