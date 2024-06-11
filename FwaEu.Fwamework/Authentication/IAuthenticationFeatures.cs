using FwaEu.Fwamework.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public interface IAuthenticationFeatures
	{
		bool EnableCredentialsAuthenticateAction { get; set; }
		bool EnableTokenRenewAction { get; set; }
		bool UseBearer { get; set; }
		List<string> AuthenticationSchemes { get; set; }
		List<Type> AuthenticationTypes { get; }
		
		IEnumerable<Type> CustomizeTypesInWhichSearchForMappings(IEnumerable<Type> types);
	}

	public class AuthenticationFeatures : IAuthenticationFeatures
	{
		public AuthenticationFeatures(
			bool enableCredentialsAuthenticateAction,
			bool enableTokenRenewAction,
			bool useBearer,
			List<Type> mappingTypes,
			List<string> authenticationSchemes)
		{
			this.EnableCredentialsAuthenticateAction = enableCredentialsAuthenticateAction;
			this.EnableTokenRenewAction = enableTokenRenewAction;
			this.UseBearer = useBearer;
			this._mappingTypes = mappingTypes ?? throw new ArgumentNullException(nameof(mappingTypes));
			this.AuthenticationSchemes = authenticationSchemes;
		}

		public bool EnableCredentialsAuthenticateAction { get; set; }
		public bool EnableTokenRenewAction { get; set; }
		public bool UseBearer { get; set; }
		private readonly List<Type> _mappingTypes;
		public List<string> AuthenticationSchemes { get; set; }

		public List<Type> AuthenticationTypes => this._mappingTypes;

		public void AddMappingType(Type mappingType)
		{
			this._mappingTypes.Add(mappingType);
		}

		public void AddMappingTypes(List<Type> mappingTypes) {
			this._mappingTypes.AddRange(mappingTypes);
		}

		public IEnumerable<Type> CustomizeTypesInWhichSearchForMappings(IEnumerable<Type> types)
		{
			return types.Where(type => !typeof(IAuthenticationClassMap).IsAssignableFrom(type))
				.Concat(this._mappingTypes);
		}
	}
}
