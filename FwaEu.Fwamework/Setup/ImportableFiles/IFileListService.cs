using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Configuration;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public interface IFileListService
	{
		IEnumerable<FileServiceModel> GetFiles();
	}

	public class FileServiceModel
	{
		public FileServiceModel(string id, long fileLengthInBytes, DateTime fileLastModificationOn,
			string filePath, string contentType, string absolutePath)
		{
			this.Id = id;
			this.FileLengthInBytes = fileLengthInBytes;
			this.FileLastModificationOn = fileLastModificationOn;
			this.FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
			this.ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
			this.AbsolutePath = absolutePath ?? throw new ArgumentNullException(nameof(absolutePath));
		}

		public string Id { get; }
		public long FileLengthInBytes { get; }
		public DateTime FileLastModificationOn { get; }
		public string FilePath { get; }
		public string ContentType { get; }
		public string AbsolutePath { get; }
	}

	public class DefaultFileListService : IFileListService
	{
		public DefaultFileListService(IOptions<SetupOptions> settings,
			IPathFileProvider filesProvider, IContentTypeProvider contentTypeProvider,
			IFileIdService fileIdService)
		{
			this._settings = (settings ?? throw new ArgumentNullException(nameof(settings))).Value;
			this._filesProvider = filesProvider ?? throw new ArgumentNullException(nameof(filesProvider));
			this._contentTypeProvider = contentTypeProvider ?? throw new ArgumentNullException(nameof(contentTypeProvider));
			this._fileIdService = fileIdService ?? throw new ArgumentNullException(nameof(fileIdService));
		}

		private readonly SetupOptions _settings;
		private readonly IPathFileProvider _filesProvider;
		private readonly IContentTypeProvider _contentTypeProvider;
		private readonly IFileIdService _fileIdService;

		private static string NormalizeFilePath(FileInfoByPathEntry file)
		{
			var prefix = "/";
			var fileRelativePath = file.FileInfo.Name;

			if (file.PathIsDirectory)
			{
				var fileFullPath = file.FileInfo.FullName.Replace("\\", "/");
				fileRelativePath = fileFullPath.Substring(file.PathEntryAbsolutePath.Length);

				var pathLastDirectory = (String.IsNullOrEmpty(file.PathEntry.Path)
						? file.PathEntryAbsolutePath
						: file.PathEntry.Path)
					.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
					.Last();

				prefix += pathLastDirectory + "/";
			}

			if (fileRelativePath.StartsWith("/"))
			{
				prefix = prefix.TrimEnd('/');
			}

			return prefix + fileRelativePath;
		}

		private string GetContentType(string filename)
		{
			var contentType = default(string);
			if (!_contentTypeProvider.TryGetContentType(filename, out contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;
		}

		public IEnumerable<FileServiceModel> GetFiles()
		{
			var browsableImportableFilesPaths = this._settings.BrowsableImportableFilesPaths;
			var files = this._filesProvider.GetFiles(browsableImportableFilesPaths);

			return files.Select(fibpe =>
			{
				return new FileServiceModel
				(
					id: this._fileIdService.GetFileId(fibpe.FileInfo.FullName),
					filePath: NormalizeFilePath(fibpe),
					fileLengthInBytes: fibpe.FileInfo.Length,
					fileLastModificationOn: fibpe.FileInfo.LastWriteTime,
					contentType: this.GetContentType(fibpe.FileInfo.Name),
					absolutePath: fibpe.FileInfo.FullName
				);
			});
		}
	}
}
