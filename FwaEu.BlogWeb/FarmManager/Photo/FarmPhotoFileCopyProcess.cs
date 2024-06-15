using FwaEu.Fwamework.Configuration;
using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.FileTransactions;
using FwaEu.Modules.FileTransactionsFileSystem;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Photo
{
	public interface IFarmPhotoFileCopyProcessFactory
	{
		string StorageRootDirectory { get; }
		FarmPhotoFileCopyProcess Create(IFile newFile, IDeletableFile existingFile);
	}

	public class DefaultFarmPhotoFileCopyProcessFactory : IFarmPhotoFileCopyProcessFactory
	{
		private readonly ICurrentDateTime _currentDateTime;
		private readonly PhotosOptions _photosOptions;

		public DefaultFarmPhotoFileCopyProcessFactory(
			ICurrentDateTime currentDateTime,
			IRootPathProvider rootPathProvider,
			IOptions<PhotosOptions> photosOptions)
		{
			this._currentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));

			_ = rootPathProvider
				?? throw new ArgumentNullException(nameof(rootPathProvider));

			this._photosOptions = photosOptions?.Value
				?? throw new ArgumentNullException(nameof(PhotosOptions));

			this._storageRootDirectory = new Lazy<string>(() =>
				rootPathProvider.GetRootPath(this._photosOptions.StoragePathRoot));
		}

		private readonly Lazy<string> _storageRootDirectory;
		public string StorageRootDirectory => this._storageRootDirectory.Value;

		public FarmPhotoFileCopyProcess Create(IFile newFile, IDeletableFile existingFile)
		{
			return new FarmPhotoFileCopyProcess(this.StorageRootDirectory,
				new ByDateStoragePathGenerator(this._currentDateTime),
				newFile, existingFile);
		}
	}

	public class FarmPhotoFileCopyProcess : FileSystemFileCopyProcess
	{
		public FarmPhotoFileCopyProcess(string rootDirectory, IStoragePathGenerator storagePathGenerator,
			IFile newFile, IDeletableFile existingFile)
			: base(rootDirectory, storagePathGenerator, newFile, existingFile)
		{
		}
	}
}
