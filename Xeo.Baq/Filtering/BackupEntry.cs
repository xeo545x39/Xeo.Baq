namespace Xeo.Baq.Filtering
{
    public class BackupEntryKey
    {
        public BackupEntryKey(string path)
            => Path = path;

        public string Path { get; }
    }

    public class BackupEntry
    {
        public BackupEntry(BackupEntryKey key, FileSystemEntryType entryType)
        {
            Key = key;
            EntryType = entryType;
            SyncObject = new object();
        }

        public BackupEntryKey Key { get; }
        public FileSystemEntryType EntryType { get; set; }
        public object SyncObject { get; }

        public override string ToString()
            => Key.Path;
    }
}