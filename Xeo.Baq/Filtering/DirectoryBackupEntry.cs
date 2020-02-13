using System.IO;

namespace Xeo.Baq.Filtering
{
    public class DirectoryBackupEntry : BackupEntry
    {
        public DirectoryBackupEntry(BackupEntryKey key, DirectoryInfo info) : base(key, FileSystemEntryType.Directory) { }
    }
}