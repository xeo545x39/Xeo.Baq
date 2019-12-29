using System.Collections.Generic;
using System.IO;

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
        public IEnumerable<FileAttributes> Attributes { get; set; }
    }
}