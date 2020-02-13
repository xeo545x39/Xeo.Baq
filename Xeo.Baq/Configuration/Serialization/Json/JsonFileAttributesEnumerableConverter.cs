using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xeo.Baq.Configuration.Serialization.Json
{
    public class JsonFileAttributesEnumerableConverter : JsonConverter<IEnumerable<FileAttributes>>
    {
        public override IEnumerable<FileAttributes> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ICollection<FileAttributes> attributes = new List<FileAttributes>();

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                // get first value
                reader.Read();

                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    string value = reader.GetString();
                    if (!Enum.TryParse(value, out FileAttributes parsedEnum))
                    {
                        throw new Exception($"Could not parse enum value '{value}' to enum of type {typeof(FileAttributes).FullName}'");
                    }

                    attributes.Add(parsedEnum);

                    // read next value
                    reader.Read();
                }

                return attributes;
            }

            throw new Exception("Provided value is not a JSON array.");
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<FileAttributes> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (FileAttributes current in value)
            {
                string enumNameValue = Enum.GetName(typeof(FileAttributes), current);

                writer.WriteStringValue(enumNameValue);
            }

            writer.WriteEndArray();
        }
    }
}