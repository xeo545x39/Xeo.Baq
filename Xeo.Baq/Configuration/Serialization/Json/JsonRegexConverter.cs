using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Xeo.Baq.Configuration.Serialization.Json
{
    public class JsonRegexConverter : JsonConverter<Regex>
    {
        public override Regex Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            var regex = new Regex(value);

            if (regex == null)
            {
                throw new Exception($"Could not parse regular expression '{value}'");
            }

            return regex;
        }

        public override void Write(Utf8JsonWriter writer, Regex value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}