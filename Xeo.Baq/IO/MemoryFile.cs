using System.IO;

namespace Xeo.Baq.IO
{
    public class MemoryFile
    {
        private MemoryFile(string path, byte[] data)
        {
            Path = path;
            Data = data;
        }

        public string Path { get; }
        public byte[] Data { get; }

        public static MemoryFile Load(string path)
        {
            byte[] content = File.ReadAllBytes(path);

            return new MemoryFile(path, content);
        }
    }
}