using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System;

namespace FwaEu.Fwamework.WebApi
{
	public class NewtonsoftMVCJsonSerializer : IMVCJsonSerializer
	{
		private readonly JsonSerializer _jsonSerializer;

		public NewtonsoftMVCJsonSerializer(IOptions<MvcNewtonsoftJsonOptions> jsonSerializerOptions)
		{
			_jsonSerializer = JsonSerializer.CreateDefault(jsonSerializerOptions.Value.SerializerSettings);
		}

		public T? DeserializeObject<T>(string value)
			=> (T?)DeserializeObject(value, typeof(T));

		public object DeserializeObject(string value, Type type)
		{
			using (JsonTextReader reader = new JsonTextReader(new StringReader(value)))
			{
				return _jsonSerializer.Deserialize(reader, type);
			}
		}
	}
}
