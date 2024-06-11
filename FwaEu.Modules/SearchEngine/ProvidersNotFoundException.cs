using FwaEu.Fwamework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{
	public class ProvidersNotFoundException : NotFoundException
	{
		public ProvidersNotFoundException(string[] providerKeys)
			: base($"Search providers not found: {String.Join(", ", providerKeys)}.")
		{
			this.ProviderKeys = providerKeys ?? throw new ArgumentNullException(nameof(providerKeys));
		}

		public string[] ProviderKeys { get; }
	}
}
