using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public abstract class DataReader
	{
		private ModelPropertyDescriptor[] _properties;

		protected abstract IEnumerable<ModelPropertyDescriptor> LoadProperties();

		public ModelPropertyDescriptor[] GetProperties()
		{
			if (this._properties == null)
			{
				this._properties = this.LoadProperties().ToArray();
			}

			return this._properties;
		}

		public abstract IEnumerable<DataRow> GetRows();
	}
}
