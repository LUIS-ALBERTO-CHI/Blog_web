using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{

	public class ModelImporterEventListeners<TModelImporterEventListener> : IModelImporterEventListener
		where TModelImporterEventListener : IModelImporterEventListener
	{

		private readonly List<TModelImporterEventListener> _listeners;
		public ModelImporterEventListeners(IEnumerable<TModelImporterEventListener> listeners)
		{
			_listeners = listeners.ToList();
		}


		public async Task OnImportFinished()
		{
			foreach (var item in _listeners)
			{
				await item.OnImportFinished();
			}
		}

		public async Task OnModelSavedAsync(object model)
		{
			foreach (var item in _listeners)
			{
				await item.OnModelSavedAsync(model);
			}
		}

		public async Task OnModelSavingAsync(object model)
		{
			foreach (var item in _listeners)
			{
				await item.OnModelSavingAsync(model);
			}
		}

		public async Task OnModelSavingErrorAsync(Exception exception)
		{
			foreach (var item in _listeners)
			{
				await item.OnModelSavingErrorAsync(exception);
			}
		}

		public async ValueTask DisposeAsync()
		{
			foreach (var item in _listeners)
			{
				try
				{
					await item.DisposeAsync();
				}
				catch
				{
					//Do not throw any exception if some dispose fails in order to prevent partial collection dispose
				}
			}
		}
	}
}
