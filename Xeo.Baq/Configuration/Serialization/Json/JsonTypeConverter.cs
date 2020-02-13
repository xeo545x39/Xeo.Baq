using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xeo.Baq.Configuration.Serialization.Json
{
    public class JsonTypeConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            var type = Type.GetType(value);

            if (type == null)
            {
                throw new TypeLoadException($"Could not find type '{value}'");
            }

            return type;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.AssemblyQualifiedName);
        }
    }
}