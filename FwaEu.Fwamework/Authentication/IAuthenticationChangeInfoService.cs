using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public interface IAuthenticationChangeInfoService
	{
		bool Enabled { get; }

		Task<DateTime?> GetLastChangeDateAsync(int userId);
		Task SetLastChangeDateAsync(int userId);
	}

	/// <summary>
	/// Will be used When the authentication initializer do not register a IAuthenticationChangeInfoService.
	/// </summary>
	public class EmptyAuthenticationChangeInfoService : IAuthenticationChangeInfoService
	{
		public bool Enabled => false;

		public Task<DateTime?> GetLastChangeDateAsync(int userId)
		{
			throw new NotSupportedException();
		}

		public Task SetLastChangeDateAsync(int userId)
		{
			return Task.CompletedTask;
		}
	}
}
