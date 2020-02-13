using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Actions
{
    public class CopyFileActionParameter : IFileSystemActionParameter
    {
        public CopyFileActionParameter(string destination, FileBackupEntry source)
        {
            Destination = destination;
            Source = source;
        }

        public string Destination { get; }
        public FileBackupEntry Source { get; }
    }
}