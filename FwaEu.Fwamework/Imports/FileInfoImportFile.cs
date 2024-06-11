using FwaEu.Fwamework.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public class FileInfoImportFile : FileInfoFileAdapter, IImportFile
	{
		public FileInfoImportFile(FileInfo fileInfo) : base(fileInfo)
		{
		}
	}
}
