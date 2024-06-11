using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Fwamework.Tests.Imports
{
	public class ImportFileFake : IImportFile
	{
		public ImportFileFake(string name)
		{
			this.Name = name;
		}

		public string Name { get; }
		public long LengthInBytes => 666;

		public Stream OpenReadStream()
		{
			throw new NotImplementedException();
		}
	}
}
