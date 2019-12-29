using Xeo.Baq.Filtering;

namespace Xeo.Baq.IO
{
    public class CopyFileActionParameter : IFileSystemActionParameter
    {
        public string Destination { get; set; }
        public BackupEntry Source { get; set; }
    }
}