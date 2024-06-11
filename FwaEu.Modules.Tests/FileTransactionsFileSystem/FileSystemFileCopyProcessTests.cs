using FwaEu.Fwamework.Data;
using FwaEu.Modules.FileTransactionsFileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.FileTransactionsFileSystem
{
	[TestClass]
	public class FileSystemFileCopyProcessTests
	{
		// NOTE: In real use, it should be an absolute path.
		//       In test context, it will relative to /bin/Debug/netcoreapp3.1

		private const string RootPath = "FileSystemFileCopyProcessTestsRoot/";

		private static void DeleteRoot()
		{
			var directory = new DirectoryInfo(RootPath);

			if (directory.Exists)
			{
				directory.Delete(true);
			}
		}

		[TestMethod]
		public async Task CopyAsync_CreateNewFile()
		{
			DeleteRoot(); // NOTE: CleanUp a possible previous failed test

			var newFile = new SourceFileStub();
			var expectedFilePath = Path.Combine(RootPath,
				FileSystemFileCopyProcessStub.GetRelativePath(newFile.Name));

			var process = new FileSystemFileCopyProcessStub(RootPath, newFile, null);
			await process.CopyAsync(CancellationToken.None);

			Assert.IsTrue(File.Exists(expectedFilePath));

			var content = File.ReadAllText(expectedFilePath);
			Assert.AreEqual(SourceFileStub.StreamContent, content,
				$"The content wrote on the FileSystem must be the same as provided by the {nameof(SourceFileStub)}.");

			DeleteRoot(); // NOTE: CleanUp
		}

		[TestMethod]
		public async Task CopyAsync_ReplaceExistingFileByNothing()
		{
			DeleteRoot(); // NOTE: CleanUp a possible previous failed test

			var existingFileInfo = CreateDummyFile();
			var existingFile = new FileInfoDeletableFileAdapter(existingFileInfo);

			Assert.IsTrue(File.Exists(existingFileInfo.FullName));

			var process = new FileSystemFileCopyProcessStub(RootPath, null, existingFile);
			await process.CopyAsync(CancellationToken.None);
			await process.DeleteExistingAsync();

			Assert.IsFalse(File.Exists(existingFileInfo.FullName));

			DeleteRoot(); // NOTE: CleanUp
		}

		[TestMethod]
		public async Task DeleteNewAsync()
		{
			DeleteRoot(); // NOTE: CleanUp a possible previous failed test

			var newFile = new SourceFileStub();
			var expectedFilePath = Path.Combine(RootPath,
				FileSystemFileCopyProcessStub.GetRelativePath(newFile.Name));

			var process = new FileSystemFileCopyProcessStub(RootPath, newFile, null);
			await process.CopyAsync(CancellationToken.None); // NOTE: DeleteNewAsync require that CopyAsync was invoked

			Assert.IsTrue(File.Exists(expectedFilePath));
			await process.DeleteNewAsync();
			Assert.IsFalse(File.Exists(expectedFilePath));

			DeleteRoot(); // NOTE: CleanUp
		}

		private static FileInfo CreateDummyFile()
		{
			var existingFileInfo = new FileInfo(Path.Combine(RootPath, "dummy.txt"));
			existingFileInfo.Directory.Create();

			using (var writer = existingFileInfo.CreateText())
			{
				writer.Write("Content not happy.");
			}

			return existingFileInfo;
		}

		[TestMethod]
		public async Task DeleteExistingAsync()
		{
			DeleteRoot(); // NOTE: CleanUp a possible previous failed test

			var newFile = new SourceFileStub();
			var existingFileInfo = CreateDummyFile();
			var existingFile = new FileInfoDeletableFileAdapter(existingFileInfo);
			var process = new FileSystemFileCopyProcessStub(RootPath, newFile, existingFile);

			Assert.IsTrue(File.Exists(existingFileInfo.FullName));
			await process.DeleteExistingAsync();
			Assert.IsFalse(File.Exists(existingFileInfo.FullName));

			DeleteRoot(); // NOTE: CleanUp
		}
	}
}
