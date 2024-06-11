using FwaEu.Modules.FileTransactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.FileTransactions
{
	public class FileCopyProcessSpy : IFileCopyProcess
	{
		public bool CopyAsyncInvoked { get; private set; } = false;
		public bool DeleteExistingAsyncInvoked { get; private set; } = false;
		public bool DeleteNewAsyncInvoked { get; private set; } = false;
		public bool DisposeAsyncInvoked { get; private set; } = false;

		public Task<FileCopyProcessResult> CopyAsync(CancellationToken cancellationToken)
		{
			this.CopyAsyncInvoked = true;
			return Task.FromResult<FileCopyProcessResult>(new FileCopyProcessResultDummy());
		}

		public Task DeleteExistingAsync()
		{
			this.DeleteExistingAsyncInvoked = true;
			return Task.CompletedTask;
		}

		public Task DeleteNewAsync()
		{
			this.DeleteNewAsyncInvoked = true;
			return Task.CompletedTask;
		}

		public ValueTask DisposeAsync()
		{
			this.DisposeAsyncInvoked = true;
			return new ValueTask();
		}
	}
}
