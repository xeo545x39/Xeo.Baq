using System.IO;

namespace Xeo.Baq.Filtering
{
    public class FileBackupEntry : BackupEntry
    {
        public FileBackupEntry(BackupEntryKey key, FileInfo info) : base(key, FileSystemEntryType.File) { }
    }
}