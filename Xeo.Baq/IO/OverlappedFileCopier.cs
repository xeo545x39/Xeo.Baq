using System;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.IO
{
    public class OverlappedFileCopier : IFileCopier
    {
        public void Copy(string source, string destination)
        {
            //WinApi.Kernel32.Kernel32Methods.CreateFile();
            throw new NotImplementedException();
        }

        public void Copy(byte[] fileContent, string destination)
        {
            throw new NotImplementedException();
        }
    }
}