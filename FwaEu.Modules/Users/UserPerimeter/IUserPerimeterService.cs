using FwaEu.Fwamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface IUserPerimeterService
	{
		Task<string[]> GetFullAccessPerimeterKeysAsync(int userId);
		Task<UserPerimeterModel[]> GetAccessesAsync(int userId);
		Task<UserPerimeterModel> GetAccessAsync(int userId, string perimeterKey);
		Task UpdatePerimeterAsync(int userId, UserPerimeterModel perimeter);
	}

	public class UserPerimeterModel
	{
		public UserPerimeterModel(string key, bool hasFullAccess, object[] accessibleIds)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.HasFullAccess = hasFullAccess;

			if (hasFullAccess && accessibleIds != null)
			{
				throw new ArgumentException(
					$"Must be null when {nameof(hasFullAccess)} is true.",
					nameof(accessibleIds));
			}
			
			if (!hasFullAccess && accessibleIds == null)
			{
				throw new ArgumentNullException(nameof(accessibleIds));
			}

			this.AccessibleIds = accessibleIds;
		}

		public string Key { get; }

		public bool HasFullAccess { get; }

		/// <summary>
		/// Will be set only if HasFullAccess == false
		/// </summary>
		public object[] AccessibleIds { get; }
	}
}
