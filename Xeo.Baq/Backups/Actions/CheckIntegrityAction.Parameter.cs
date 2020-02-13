using System.IO;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Actions
{
    public class CheckIntegrityActionParameter : IFileSystemActionParameter
    {
        public CheckIntegrityActionParameter(string file1Path, string file2Path)
        {
            File1 = new FileInfo(file1Path);
            File2 = new FileInfo(file2Path);
        }

        public CheckIntegrityActionParameter(MemoryFile file1, string file2Path)
        {
            LoadedFile1 = file1;
            File1 = new FileInfo(file1.Path);
            File2 = new FileInfo(file2Path);
        }

        public FileInfo File1 { get; }
        public FileInfo File2 { get; }
        public MemoryFile LoadedFile1 { get; }
    }
}