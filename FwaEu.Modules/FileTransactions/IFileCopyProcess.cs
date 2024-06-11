using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.FileTransactions
{
	public interface IFileCopyProcess : IAsyncDisposable
	{
		Task<FileCopyProcessResult> CopyAsync(CancellationToken cancellationToken);

		Task DeleteNewAsync();
		Task DeleteExistingAsync();
	}

	public class FileCopyProcessResult
	{
		public FileCopyProcessResult(string relativePath)
		{
			this.RelativePath = relativePath; //NOTE: RelativePath will be null when no file is copied
		}

		public string RelativePath { get; }
	}
}
