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
        public BackupEntry(BackupEntryKey key)
        {
            Key = key;
            SyncObject = new object();
        }

        public BackupEntryKey Key { get; }
        public object SyncObject { get; }

        public override string ToString()
            => Key.Path;
    }
}