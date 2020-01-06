using System;

namespace Xeo.Baq.IO
{
    public class OverlappedFileCopier : IFileCopier
    {
        public void Copy(string source, string destination)
        {
            //WinApi.Kernel32.Kernel32Methods.CreateFile();
            throw new NotImplementedException();
        }
    }
}