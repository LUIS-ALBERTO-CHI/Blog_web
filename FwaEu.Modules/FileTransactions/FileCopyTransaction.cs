using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.FileTransactions
{
	public class FileCopyTransaction : IAsyncDisposable
	{
		public FileCopyTransaction(params IFileCopyProcess[] fileCopyProcesses)
		{
			this.FileCopyProcesses = fileCopyProcesses
				?? throw new ArgumentNullException(nameof(fileCopyProcesses));
		}

		protected IFileCopyProcess[] FileCopyProcesses { get; }

		public bool Committed { get; private set; } = false;
		public bool Finished { get; private set; } = false;

		private bool _disposed = false;
		private bool _copyAllInvoked = false;

		public virtual async Task<Dictionary<IFileCopyProcess, FileCopyProcessResult>> CopyAllAsync(CancellationToken cancellationToken)
		{
			var results = new Dictionary<IFileCopyProcess, FileCopyProcessResult>(this.FileCopyProcesses.Length);

			if (this._copyAllInvoked)
			{
				throw new NotSupportedException(
					$"{nameof(CopyAllAsync)} is invokable only once per transaction.");
			}

			foreach (var process in this.FileCopyProcesses)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var result = await process.CopyAsync(cancellationToken);
				results.Add(process, result);
			}

			this._copyAllInvoked = true;
			return results;
		}

		private void ThrowIfCopyAllNotInvoked()
		{
			if (!this._copyAllInvoked)
			{
				throw new NotSupportedException(
					$"{nameof(CopyAllAsync)} must be invoked before.");
			}
		}

		public virtual async Task CommitAsync()
		{
			this.ThrowIfCopyAllNotInvoked();

			foreach (var process in this.FileCopyProcesses)
			{
				await process.DeleteExistingAsync();
			}

			this.Committed = true;
		}

		public virtual async Task RollbackAsync()
		{
			foreach (var process in this.FileCopyProcesses)
			{
				await process.DeleteNewAsync();
			}
		}

		private async Task GetDisposeAsyncTask()
		{
			if (!this.Committed && !this.Finished)
			{
				await this.RollbackAsync();
			}

			foreach (var process in this.FileCopyProcesses)
			{
				await process.DisposeAsync();
			}

			this.Finished = true;
		}

		public ValueTask DisposeAsync()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("Already disposed.");
			}

			this._disposed = true;
			return new ValueTask(this.GetDisposeAsyncTask());
		}
	}
}
