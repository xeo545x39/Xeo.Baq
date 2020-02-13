namespace Xeo.Baq.IO.Abstract
{
    public interface IFileCopier
    {
        void Copy(string source, string destination);
        void Copy(byte[] fileContent, string destination);
    }
}