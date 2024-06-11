using FwaEu.Fwamework.Data;
using FwaEu.Modules.FileTransactions;
using FwaEu.Modules.FileTransactionsFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.FileTransactionsFileSystem
{
	public class FileSystemFileCopyProcessStub : FileSystemFileCopyProcess
	{
		private class StoragePathGeneratorStub : IStoragePathGenerator
		{
			public string GetStorageRelativePath(string fileName)
			{
				return GetRelativePath(fileName);
			}
		}

		public FileSystemFileCopyProcessStub(string rootDirectory, IFile newFile, IDeletableFile existingFile)
			: base(rootDirectory, new StoragePathGeneratorStub(), newFile, existingFile)
		{
		}

		public static string GetRelativePath(string fileName)
		{
			return String.Format("SubDirectory/1234_{0}", fileName);
		}
	}
}
