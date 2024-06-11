using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public interface IImportEventListener
	{
		Task OnDisposingAsync();
	}
}
