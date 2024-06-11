using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Caching
{
	public interface ICacheManager
	{
		Task ClearAsync();
	}
}
