using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Xeo.Baq.Configuration.Serialization.Json;

namespace Xeo.Baq.Filtering
{
    public class FileSystemFilter
    {
        public string Path { get; set; }
        public string NameMask { get; set; }

        /// <summary>
        ///     Regex has priority over <seealso cref="NameMask" /> property.
        /// </summary>
        public string Regex { get; set; }

        public FileSystemEntryType EntryType { get; set; }

        [JsonConverter(typeof(JsonFileAttributesEnumerableConverter))]
        public IEnumerable<FileAttributes> Attributes { get; set; }
    }
}