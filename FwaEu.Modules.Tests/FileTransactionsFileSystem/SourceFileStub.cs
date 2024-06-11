using FwaEu.Fwamework.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Modules.Tests.FileTransactionsFileSystem
{
	public class SourceFileStub : IFile
	{
		public const string StreamContent = "Romain-Romain";
		private readonly byte[] _bytes = Encoding.UTF8.GetBytes(StreamContent);

		public string Name => "source-stub.txt";

		public long LengthInBytes => this._bytes.Length;

		public Stream OpenReadStream()
		{
			return new MemoryStream(this._bytes);
		}
	}
}
