using System.IO;

namespace Xeo.Baq.IO
{
    public class ManagedFileCopier : IFileCopier
    {
        public void Copy(string source, string destination)
        {
            File.Copy(source, destination);
        }
    }
}