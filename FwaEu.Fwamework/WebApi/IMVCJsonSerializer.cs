using System;

namespace FwaEu.Fwamework.WebApi
{
	/// <summary>
	/// This interface will serialize/deserialize JSON like the ASP controllers would do.
	/// TODO: https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/11223
	/// </summary>
	public interface IMVCJsonSerializer
	{
		public object DeserializeObject(string value, Type type);
		public T? DeserializeObject<T>(string value);
	}
}
