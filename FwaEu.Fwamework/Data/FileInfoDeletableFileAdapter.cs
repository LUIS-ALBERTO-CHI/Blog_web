using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data
{
	public class FileInfoDeletableFileAdapter : FileInfoFileAdapter, IDeletableFile
	{
		public FileInfoDeletableFileAdapter(FileInfo fileInfo) : base(fileInfo)
		{
		}

		public Task DeleteAsync()
		{
			// NOTE: Synchronous deletion, could by replaced by:
			// using (new FileStream(this.FileInfo.FullName, FileMode.Open, FileAccess.Read,
			//    FileShare.None, 1, FileOptions.DeleteOnClose | FileOptions.Asynchronous));

			this.FileInfo.Delete();
			return Task.CompletedTask;
		}
	}
}
