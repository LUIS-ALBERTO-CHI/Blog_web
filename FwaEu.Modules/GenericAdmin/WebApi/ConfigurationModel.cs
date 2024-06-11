using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class ConfigurationModel
	{
		public PropertyModel[] Properties { get; set; }
		public AuthorizedActionsModel AuthorizedActions { get; set; }

		public static ConfigurationModel FromConfiguration(IGenericAdminModelConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var authorizedActions = configuration.GetAuthorizedActions();

			return new ConfigurationModel
			{
				Properties = configuration.GetProperties().Select(p =>
				{
					var propertyModel = new PropertyModel
					{
						Name = p.Name,
						Type = p.CustomInnerTypeName ?? p.InnerType.Name,
						ExtendedProperties = p.ExtendedProperties,
					};

					Property.Copy(p, propertyModel);
					return propertyModel;
				})
				.ToArray(),

				AuthorizedActions = new AuthorizedActionsModel
				{
					AllowCreate = authorizedActions.AllowCreate,
					AllowDelete = authorizedActions.AllowDelete,
					AllowUpdate = authorizedActions.AllowUpdate,
				}
			};
		}
	}

	public class PropertyModel : IProperty
	{
		public bool IsKey { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }

		public bool IsEditable { get; set; } = true;
		public Dictionary<string, object> ExtendedProperties { get; set; }
	}

	public class AuthorizedActionsModel
	{
		public bool AllowCreate { get; set; }
		public bool AllowUpdate { get; set; }
		public bool AllowDelete { get; set; }
	}
}