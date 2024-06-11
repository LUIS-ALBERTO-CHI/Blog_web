using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FwaEu.Fwamework.WebApi;
using FwaEu.Modules.Data.Database;
using FwaEu.Fwamework.Globalization;
using Newtonsoft.Json.Linq;
using System;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class GenericAdminController : ControllerBase
	{
		private static DataSourceModel CreateDataSourceModel(
				IEnumerable<IDataSourceModelFactory> dataSourceModelFactories, IDataSource dataSource)
		{
			return dataSourceModelFactories.Select(factory => factory.Create(dataSource))
				.First(model => model != null);
		}

		private static GenericAdminPaginationParameters BuildPaginationParameters(PaginationParametersModel pagination)
		{
			return pagination == null ? null : new GenericAdminPaginationParameters
			(
				pagination.RequireTotalCount == true,
				pagination.Skip,
				pagination.Take
			);
		}

		private static GenericAdminSortParameters BuildSortParameters(SortParametersModel sort)
		{
			return sort?.Parameters?.Count > 0 ? new GenericAdminSortParameters
			(
				new List<GenericAdminSortParameter>
				(
					sort.Parameters.Select(o => new GenericAdminSortParameter
					(
						CamelToPascal(o.ColumnName),
						o.Ascending == true
					))
				)
			) : null;
		}

		private static GenericAdminFilterMode StringToFilterMode(string mode)
		{
			switch (mode)
			{
				case "and": return GenericAdminFilterMode.And;
				case "or": return GenericAdminFilterMode.Or;
				case "contains": return GenericAdminFilterMode.Contains;
				case "notcontains": return GenericAdminFilterMode.NotContains;
				case "startswith": return GenericAdminFilterMode.StartsWith;
				case "endswith": return GenericAdminFilterMode.EndsWith;
				case "=": return GenericAdminFilterMode.Equals;
				case "<>": return GenericAdminFilterMode.NotEquals;
				case "<": return GenericAdminFilterMode.LessThan;
				case ">": return GenericAdminFilterMode.GreaterThan;
				case "<=": return GenericAdminFilterMode.LessOrEqual;
				case ">=": return GenericAdminFilterMode.GreaterOrEqual;
				default: throw new NotImplementedException($"Filter mode '{mode}' is not implemented");
			}
		}

		private static object JTokenToValue(JToken token)
		{
			switch (token.Type)
			{
				case JTokenType.None:
				case JTokenType.Undefined:
				case JTokenType.Null:
					return null;

				case JTokenType.String:
				case JTokenType.Uri:
					return token.Value<string>();

				case JTokenType.Boolean: return token.Value<bool>();
				case JTokenType.Float: return token.Value<decimal>();
				case JTokenType.Date: return token.Value<DateTime>();
				case JTokenType.Integer: return token.Value<long>();
				case JTokenType.Guid: return token.Value<Guid>();

				default: throw new NotImplementedException($"Unhandled type '{token.Type}'");
			}
		}

		private static GenericAdminFilterConditionContainer BuildFiltersList(JArray filters)
		{
			var result = new GenericAdminFilterConditionContainer();

			if (!filters.Any(t => t is JArray))
			{
				result.Condition = new GenericAdminFilterCondition
				{
					ColumnName = CamelToPascal(filters[0].Value<string>()),
					Mode = StringToFilterMode(filters[1].Value<string>()),
					Value = JTokenToValue(filters[2])
				};
			}
			else
			{
				result.Container = new List<GenericAdminFilterConditionContainerValue>();

				for (var i = 0; i < filters.Count; ++i)
				{
					if (filters[i] is not JArray)
						throw new InvalidOperationException();

					result.Container.Add(new GenericAdminFilterConditionContainerValue
					{
						Container = BuildFiltersList(filters[i++] as JArray),
						ContainerMode = i < filters.Count ? StringToFilterMode(filters[i].Value<string>()) : GenericAdminFilterMode.None
					});
				}
			}

			return result;
		}

		private static GenericAdminFilterParameters BuildFilterParameters(FilterParametersModel filter)
		{
			return filter == null || filter.Filters == null || filter.Filters.Count < 1 ? null : new GenericAdminFilterParameters
			{
				Filters = BuildFiltersList(filter.Filters)
			};
		}

		private static string CamelToPascal(string value)
		{
			return value.Substring(0, 1).ToUpper() + value.Substring(1);
		}

		private static object RecursiveCamelToPascalValue(object value)
		{
			if (value != null)
			{
				if (value is Dictionary<string, object>) //NOTE: Must be before ICollection test
				{
					return RecursiveCamelToPascal((Dictionary<string, object>)value);
				}

				if (value is ICollection) //NOTE: Not IEnumerable to avoid types like string
				{
					return ((ICollection)value)
						.Cast<object>()
						.Select(v => RecursiveCamelToPascalValue(v))
						.ToArray();
				}
			}

			return value;
		}

		private static Dictionary<string, object> RecursiveCamelToPascal(Dictionary<string, object> dictionary)
		{
			return dictionary.ToDictionary(
				kv => CamelToPascal(kv.Key),
				kv => RecursiveCamelToPascalValue(kv.Value));
		}

		private static Dictionary<string, object>[] RecursiveCamelToPascal(Dictionary<string, object>[] dictionaries)
		{
			return dictionaries.Select(RecursiveCamelToPascal).ToArray();
		}

		[HttpPost("GetConfiguration/{key}")]
		[ProducesResponseType(typeof(ConfigurationModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ConfigurationModel>> GetConfiguration(string key,
			[FromServices] IGenericAdminService genericAdminService)
		{
			var configuration = genericAdminService.GetConfiguration(key);

			if (configuration == null)
				return KeyNotfound(key);

			if (!await configuration.IsAccessibleAsync())
				return Forbid("Configuration not accessible for current user.");

			return Ok(ConfigurationModel.FromConfiguration(configuration));
		}

		[HttpPost("GetModels/{key}")]
		[ProducesResponseType(typeof(DataSourceModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<DataSourceModel>> GetModels(string key,
			[FromBody] GetModelsParametersModel options,
			[FromServices] IGenericAdminService genericAdminService,
			[FromServices] IEnumerable<IDataSourceModelFactory> dataSourceModelFactories,
			[FromServices] IUserContextLanguage userContextLanguage)
		{
			var configuration = genericAdminService.GetConfiguration(key);

			if (configuration == null)
				return KeyNotfound(key);

			if (!await configuration.IsAccessibleAsync())
			{
				return Forbid("Configuration not accessible for current user.");
			}

			var loadDataResult = await configuration.GetModelsAsync(new GenericAdminGetModelsParameters
			(
				userContextLanguage.GetCulture(),
				BuildPaginationParameters(options?.Pagination),
				BuildSortParameters(options?.Sort),
				BuildFilterParameters(options?.Filter)
			));

			return Ok(CreateDataSourceModel(dataSourceModelFactories, loadDataResult.Value));
		}

		[HttpPost("Save/{configurationKey}")]
		[ProducesResponseType(typeof(SaveResultsModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
		public async Task<ActionResult<SaveResultsModel>> Save(string configurationKey, 
			[FromBody] SaveContextModel context,
			[FromServices] IGenericAdminService genericAdminService,
			[FromServices] IModelValidationService validationService,
			[FromServices] IMVCJsonSerializer jsonSerializer)
		{
			var configuration = genericAdminService.GetConfiguration(configurationKey);

			if (configuration == null)
			{
				return KeyNotfound(configurationKey);
			}

			if (!await configuration.IsAccessibleAsync())
			{
				return Forbid("Configuration not accessible for current user.");
			}

			var models = (IEnumerable<object>)jsonSerializer.DeserializeObject(context.Models.ToString(),
				(configuration.ModelType == null ? typeof(Dictionary<string, object>) : configuration.ModelType).MakeArrayType());

			foreach (var model in models)
			{
				var validationErrors = validationService.Validate(model);
				if (validationErrors.Length != 0)
				{
					return BadRequest(validationErrors);
				}
			}

			if (configuration.ModelType == null)
			{
				models = RecursiveCamelToPascal(models.Cast<Dictionary<string, object>>().ToArray());
			}

			try
			{
				return Ok(SaveResultsModel.FromSaveResults(await configuration.SaveAsync(models)));
			}
			catch (DatabaseException ex)
			{
				return Conflict(ex.Type);
			}
		}

		[HttpPost("Delete/{configurationKey}")]
		[ProducesResponseType(typeof(DeleteResultsModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
		public async Task<ActionResult<DeleteResultsModel>> Delete(string configurationKey, 
			[FromBody] DeleteContextModel context,
			[FromServices] IGenericAdminService genericAdminService,
			[FromServices] IMVCJsonSerializer jsonSerializer)
		{
			var configuration = genericAdminService.GetConfiguration(configurationKey);

			if (configuration == null)
				return KeyNotfound(configurationKey);

			if (!await configuration.IsAccessibleAsync())
				return Forbid("Configuration not accessible for current user.");

			var camelDictionaries = jsonSerializer.DeserializeObject<Dictionary<string, object>[]>(
				context.Keys.ToString());

			var deleteResult = default(DeleteResult);
			try
			{
				deleteResult = await configuration.DeleteAsync(RecursiveCamelToPascal(camelDictionaries));
			}
			catch (DatabaseException ex)
			{
				return Conflict(ex.Type);
			}

			return Ok(DeleteResultsModel.FromDeleteResults(deleteResult));
		}


		private NotFoundObjectResult KeyNotfound(string key)
		{
			return NotFound("No generic admin registered with this key: " + key);
		}
	}
}