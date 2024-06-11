using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public class DataRow
	{
		public DataRow(
			ModelPropertyDescriptor[] metadataProperties,
			Dictionary<string, object> valuesByPropertyName)
		{
			this.MetadataProperties = metadataProperties
				?? throw new ArgumentNullException(nameof(metadataProperties));

			this.ValuesByPropertyName = valuesByPropertyName
				?? throw new ArgumentNullException(nameof(valuesByPropertyName));
		}

		public ModelPropertyDescriptor[] MetadataProperties { get; }
		public Dictionary<string, object> ValuesByPropertyName { get; }
	}
}
