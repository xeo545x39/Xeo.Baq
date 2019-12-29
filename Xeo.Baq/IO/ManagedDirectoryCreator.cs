using System.IO;

namespace Xeo.Baq.IO
{
    public class ManagedDirectoryCopier : ISystemFileCopier
    {
        public void Copy(string source, string destination)
        {
            Directory.CreateDirectory()
        }
    }
}