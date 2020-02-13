namespace Xeo.Baq.IO
{
    public class DummyFileCopier : IFileCopier
    {
        public void Copy(string source, string destination) { }

        public void Copy(byte[] fileContent, string destination) { }
    }
}