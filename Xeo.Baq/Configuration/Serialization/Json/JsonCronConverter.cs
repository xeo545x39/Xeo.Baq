using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NCrontab;

namespace Xeo.Baq.Configuration.Serialization.Json
{
    public class JsonCronConverter : JsonConverter<CrontabSchedule>
    {
        public override CrontabSchedule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            CrontabSchedule cron = CrontabSchedule.TryParse(value);

            if (cron == null)
            {
                throw new CrontabException($"Could not parse cron expression '{value}'");
            }

            return cron;
        }

        public override void Write(Utf8JsonWriter writer, CrontabSchedule value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}