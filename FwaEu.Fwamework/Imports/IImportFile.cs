using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public interface IImportFile
	{
		string Name { get; }
		long LengthInBytes { get; }
		Stream OpenReadStream();
	}
}
