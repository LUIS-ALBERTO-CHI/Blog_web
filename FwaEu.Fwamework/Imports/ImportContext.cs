using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public class ImportContext : IAsyncDisposable
	{
		public ImportContext(ProcessResult processResult, IImportFile[] files)
		{
			this.ProcessResult = processResult ?? throw new ArgumentNullException(nameof(processResult));
			this.Files =  files ?? throw new ArgumentNullException(nameof(files));
		}

		public ProcessResult ProcessResult { get; }
		public ServiceStore ServiceStore { get; } = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped);
		public IImportFile[] Files { get; }

		private readonly HashSet<IImportEventListener> _listeners = new HashSet<IImportEventListener>();

		public void AddListener(IImportEventListener listener)
		{
			this._listeners.Add(listener);
		}

		public async ValueTask DisposeAsync()
		{
			this.ServiceStore.Dispose();

			await Task.WhenAll(
				this._listeners.Select(l => l.OnDisposingAsync()));
		}
	}
}
