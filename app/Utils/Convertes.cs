
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace transport_sim_app.Convert
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ"));
        }
    }public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(TimeSpan));
            return DateTime.Parse(reader.GetString()).TimeOfDay;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            var dt = new DateTime();
            dt.Add(value);
            writer.WriteStringValue(dt.ToString("'0000T'HH':'mm':'ss'.'fff"));
        }
    }
}