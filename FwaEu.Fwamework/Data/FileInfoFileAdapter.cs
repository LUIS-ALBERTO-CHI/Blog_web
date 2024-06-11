using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Fwamework.Data
{
	public class FileInfoFileAdapter : IFile
	{
		public FileInfoFileAdapter(FileInfo fileInfo)
		{
			this.FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
		}

		public FileInfo FileInfo { get; }

		public string Name => this.FileInfo.Name;
		public long LengthInBytes => this.FileInfo.Length;

		public Stream OpenReadStream()
		{
			return this.FileInfo.OpenRead();
		}
	}
}
