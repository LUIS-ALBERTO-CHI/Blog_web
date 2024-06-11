using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data
{
	public class InMemoryFile : IFile
	{
		/// <param name="fileName">If null, file.Name will be used instead.</param>
		public static async Task<InMemoryFile> CopyFromFileAsync(IFile file, string fileName = null)
		{
			var memoryStream = new MemoryStream();
			using (var fileStream = file.OpenReadStream())
			{
				await fileStream.CopyToAsync(memoryStream);
			}

			return new InMemoryFile(memoryStream, fileName ?? file.Name);
		}

		public InMemoryFile(MemoryStream memoryStream, string name)
		{
			this.MemoryStream = memoryStream ?? throw new ArgumentNullException(nameof(memoryStream));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public MemoryStream MemoryStream { get; }
		public string Name { get; }
		public long LengthInBytes => this.MemoryStream.Length;

		public Stream OpenReadStream()
		{
			this.MemoryStream.Position = 0;
			return this.MemoryStream;
		}
	}
}
