using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IPartHandler
	{
		/// <summary>
		/// PascalCasing must be used for part handler name.
		/// </summary>
		string Name { get; }
		bool IsEditable { get; }
		bool IsRequiredOnCreation { get; }
		bool IsRequiredOnUpdate { get; }
		bool IsRequiredOnUpdateForCurrentUser { get; }

		/// <summary>
		/// If returns null, the model in the SaveAsync method will be
		/// provided as a System.Collections.Generic.Dictionary<string, object>.
		/// </summary>
		Type SaveModelType { get; }

		Task<object> LoadAsync();
		Task<IPartSaveResult> SaveAsync(object model);

		Task<bool> CurrentUserCanEditAsync();
	}

	public abstract class ReadOnlyPartHandler<TModel> : IPartHandler
		where TModel : class
	{
		public abstract string Name { get; }
		public bool IsEditable => false;
		public Type SaveModelType => throw new NotImplementedException();
		public bool IsRequiredOnCreation => false;
		public bool IsRequiredOnUpdate => false;
		public bool IsRequiredOnUpdateForCurrentUser => this.IsRequiredOnUpdate;

		public Task<bool> CurrentUserCanEditAsync()
		{
			return Task.FromResult(false);
		}

		public abstract Task<TModel> LoadAsync();

		public Task<IPartSaveResult> SaveAsync(object model)
		{
			throw new NotImplementedException();
		}

		async Task<object> IPartHandler.LoadAsync()
		{
			return await this.LoadAsync();
		}
	}

	public abstract class EditablePartHandler<TModel, TSaveModel> : IPartHandler
		where TModel : class
		where TSaveModel : class
	{
		public abstract string Name { get; }
		public bool IsEditable => true;
		public Type SaveModelType => typeof(TSaveModel);
		public virtual bool IsRequiredOnCreation => false;
		public virtual bool IsRequiredOnUpdate => false;
		public virtual bool IsRequiredOnUpdateForCurrentUser => this.IsRequiredOnUpdate;

		public abstract Task<TModel> LoadAsync();
		public abstract Task<IPartSaveResult> SaveAsync(TSaveModel model);

		async Task<IPartSaveResult> IPartHandler.SaveAsync(object model)
		{
			return await this.SaveAsync((TSaveModel)model);
		}

		async Task<object> IPartHandler.LoadAsync()
		{
			return await this.LoadAsync();
		}
		public abstract Task<bool> CurrentUserCanEditAsync();
	}
}
