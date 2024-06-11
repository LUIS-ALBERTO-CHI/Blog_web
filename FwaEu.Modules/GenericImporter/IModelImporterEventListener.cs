using FwaEu.Fwamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public interface IModelImporterEventListenerFactory<TEventListener>
		where TEventListener : IModelImporterEventListener
	{
		TEventListener Create(ServiceStore serviceStore, IServiceProvider serviceProvider);
	}

	public interface IModelImporterEventListener : IAsyncDisposable
	{
		Task OnModelSavingAsync(object model);
		Task OnModelSavedAsync(object model);
		Task OnModelSavingErrorAsync(Exception exception);
		Task OnImportFinished();
	}

	public class EmptyModelImporterEventListener : IModelImporterEventListener
	{
		public virtual Task OnModelSavingAsync(object model)
		{
			return Task.CompletedTask;
		}

		public virtual Task OnModelSavedAsync(object model)
		{
			return Task.CompletedTask;
		}
		public virtual Task OnModelSavingErrorAsync(Exception exception)
		{
			return Task.CompletedTask;
		}

		public virtual Task OnImportFinished()
		{
			return Task.CompletedTask;
		}

		public virtual ValueTask DisposeAsync()
		{
			return default(ValueTask);
		}
	}
}
