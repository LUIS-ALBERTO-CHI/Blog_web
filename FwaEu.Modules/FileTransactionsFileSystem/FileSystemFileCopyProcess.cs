using FwaEu.Fwamework.Data;
using FwaEu.Modules.FileTransactions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.FileTransactionsFileSystem
{
	public class FileSystemFileCopyProcess : IFileCopyProcess
	{
		public FileSystemFileCopyProcess(
			string rootDirectory, IStoragePathGenerator storagePathGenerator,
			IFile newFile, IDeletableFile existingFile)
		{
			this.RootDirectory = rootDirectory
				?? throw new ArgumentNullException(nameof(rootDirectory));

			this.StoragePathGenerator = storagePathGenerator
				?? throw new ArgumentNullException(nameof(storagePathGenerator));

			this.NewFile = newFile;
			this.ExistingFile = existingFile;
		}

		public string RootDirectory { get; }
		protected IStoragePathGenerator StoragePathGenerator { get; }
		public IFile NewFile { get; private set; }
		public IDeletableFile ExistingFile { get; private set; }

		private bool _copyInvoked = false;
		private string _copiedFileFullPath;

		protected string GetRelativePath()
		{
			return this.StoragePathGenerator.GetStorageRelativePath(this.NewFile.Name);
		}

		protected async Task CopyAsync(string relativePath, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (this._copyInvoked)
			{
				throw new NotSupportedException(
					$"{nameof(CopyAsync)} is invokable only once per instance.");
			}

			var fullPath = Path.Combine(this.RootDirectory, relativePath);
			new FileInfo(fullPath).Directory.Create();

			using (var sourceStream = this.NewFile.OpenReadStream())
			{
				using (var destinationStream = new FileStream(fullPath,
					FileMode.Create, FileAccess.Write, FileShare.None))
				{
					await sourceStream.CopyToAsync(destinationStream, cancellationToken);
				}
			}

			this._copiedFileFullPath = fullPath;
			this._copyInvoked = true;
		}

		public async Task<FileCopyProcessResult> CopyAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			
			if (this.NewFile == null)
			{
				return new FileCopyProcessResult(null);
			}

			var relativePath = this.GetRelativePath();
			await this.CopyAsync(relativePath, cancellationToken);
			return new FileCopyProcessResult(relativePath);
		}

		public async Task DeleteNewAsync()
		{
			// NOTE: No need to check here is CopyAsync was called, because it could be invoked
			//       when a cancellationToken is cancelled during CopyAllAsync in transaction

			if (this._copiedFileFullPath == null)
			{
				return;
			}

			var fileInfo = new FileInfo(this._copiedFileFullPath);

			if (fileInfo.Exists)
			{
				var deletableFile = new FileInfoDeletableFileAdapter(fileInfo);
				await deletableFile.DeleteAsync();
			}
		}

		public async Task DeleteExistingAsync()
		{
			if (this.ExistingFile != null)
			{
				await this.ExistingFile.DeleteAsync();
			}
		}

		public ValueTask DisposeAsync()
		{
			this.NewFile = null;
			this.ExistingFile = null;

			return new ValueTask();
		}
	}
}
