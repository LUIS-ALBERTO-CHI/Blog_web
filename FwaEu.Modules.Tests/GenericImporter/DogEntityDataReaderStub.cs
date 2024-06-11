using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.Tests.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter
{
	public class DogEntityDataReaderStub : DataReaderStub<DogEntity>
	{
		public DogEntityDataReaderStub(DogEntity[] data) : base(data)
		{
			this._properties = new[]
			{
			new ModelPropertyDescriptor(nameof(DogEntity.Name), IsKeyValue.True, false, null, null),
			new ModelPropertyDescriptor(nameof(DogEntity.Price), IsKeyValue.False, false, null, null),
			new ModelPropertyDescriptor(nameof(DogEntity.Birthdate), IsKeyValue.False, false, null, null)
			};
		}

		public override Dictionary<string, object> ToDictionary(DogEntity dog)
		{
			var dictionary = new Dictionary<string, object>();

			dictionary.Add(nameof(DogEntity.Name), dog.Name);
			dictionary.Add(nameof(DogEntity.Price), dog.Price);
			dictionary.Add(nameof(DogEntity.Birthdate), dog.Birthdate);

			return dictionary;
		}
	}
}
