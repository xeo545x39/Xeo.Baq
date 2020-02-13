using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Actions
{
    public class CreateDirectoryActionParameter : IFileSystemActionParameter
    {
        public CreateDirectoryActionParameter(string destination)
            => Destination = destination;

        public string Destination { get; }
    }
}