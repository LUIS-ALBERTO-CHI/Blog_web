using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public class Base64ImportFile : IImportFile
	{
		public Base64ImportFile(string name, string content)
		{
			this.Name = name;
			this.Content = ConvertBase64ToUTF8(content);
			this.LengthInBytes = this.Content.Length;
		}

		public string Name { get; }
		public byte[] Content { get; }

		public long LengthInBytes { get; }

		public Stream OpenReadStream()
		{
			return new MemoryStream(this.Content);
		}

		private byte[] ConvertBase64ToUTF8(string content)
		{
			return Convert.FromBase64String(content);
		}
		
	}
}
