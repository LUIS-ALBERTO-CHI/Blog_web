using FwaEu.Modules.GenericImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class DataReaderStub<T> : DataReader
	{
		public DataReaderStub(T[] data)
		{
			this._data = data ?? throw new ArgumentNullException(nameof(data));
		}
 
		private readonly T[] _data;
		public virtual ModelPropertyDescriptor[] _properties { get; set; }

		public virtual Dictionary<string, object> ToDictionary(T entity)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<DataRow> GetRows()
		{
			return this._data.Select(model =>
				new DataRow(this._properties, ToDictionary(model)));
		}

		protected override IEnumerable<ModelPropertyDescriptor> LoadProperties()
		{
			return this._properties;
		}
	}
}
