using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IListPartHandler
	{
		/// <summary>
		/// PascalCasing must be used for part handler name.
		/// </summary>
		string Name { get; }

		Task<object> LoadAsync(UserListModel model);
	}

	public abstract class ListPartHandler<TModel> : IListPartHandler
		where TModel : class
	{
		public abstract string Name { get; }

		public abstract Task<TModel> LoadAsync(UserListModel model);

		async Task<object> IListPartHandler.LoadAsync(UserListModel model)
		{
			return await this.LoadAsync(model);
		}
	}
}
