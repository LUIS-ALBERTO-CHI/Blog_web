using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Fwamework.Data
{
	public interface IFile
	{
		public string Name { get; }
		public long LengthInBytes { get; }
		public Stream OpenReadStream();
	}
}
