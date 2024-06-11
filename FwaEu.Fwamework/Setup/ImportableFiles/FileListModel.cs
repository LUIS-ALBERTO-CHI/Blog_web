using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public class FileListModel
	{
		public FileListModel(FileModel[] files)
		{
			this.Files = files ?? throw new ArgumentNullException(nameof(files));
		}

		public FileModel[] Files { get; }
	}

	public class FileModel
	{
		public FileModel(string id, long fileLengthInBytes, DateTime fileLastModificationOn,
			string filePath, string contentType)
		{
			this.Id = id;
			this.FileLengthInBytes = fileLengthInBytes;
			this.FileLastModificationOn = fileLastModificationOn;
			this.FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
			this.ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
		}

		public string Id { get; }
		public long FileLengthInBytes { get; }
		public DateTime FileLastModificationOn { get; }
		public string FilePath { get; }
		public string ContentType { get; }
	}
}
