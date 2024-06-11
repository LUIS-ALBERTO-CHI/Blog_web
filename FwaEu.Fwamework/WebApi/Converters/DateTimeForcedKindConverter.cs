using Newtonsoft.Json;
using System;

namespace FwaEu.Fwamework.WebApi.Converters
{
	public class DateTimeForcedKindConverter : JsonConverter<DateTime?>
	{
		private readonly DateTimeKind _kind;

		public DateTimeForcedKindConverter(DateTimeKind kind)
		{
			_kind = kind;
		}

		public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			DateTime? dateTime = null;
			if (reader.TokenType == JsonToken.String && DateTime.TryParse(reader.Value?.ToString(), out var parsedDateTime))
			{
				dateTime = parsedDateTime;
			}
			else if (reader.TokenType == JsonToken.Date)
			{
				dateTime = (DateTime?)reader.Value;
			}
			if (dateTime.HasValue)
			{
				if (_kind != dateTime.Value.Kind)
				{
					if (_kind == DateTimeKind.Unspecified)
						dateTime = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second, DateTimeKind.Unspecified);
					else if (_kind == DateTimeKind.Local)
						dateTime = dateTime.Value.ToLocalTime();
					else
						dateTime = dateTime.Value.ToUniversalTime();
				}
			}

			return dateTime;
		}

		public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
		{
			if (value?.Kind == DateTimeKind.Unspecified)
			{
				value = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, value.Value.Hour, value.Value.Minute, value.Value.Second, _kind);
			}
			writer.WriteValue(value);
		}
	}
}
