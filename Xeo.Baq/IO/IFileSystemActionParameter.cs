using Xeo.Baq.Filtering;

namespace Xeo.Baq.IO
{
    public interface IFileSystemActionParameter
    {
        BackupEntry Source { get; set; }
    }
}