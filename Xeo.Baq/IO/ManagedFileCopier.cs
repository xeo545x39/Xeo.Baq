using System.IO;
using NLog;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.IO
{
    public class ManagedFileCopier : IFileCopier
    {
        private readonly ILogger _logger;

        public ManagedFileCopier(ILogger logger)
            => _logger = logger;

        public void Copy(string source, string destination)
        {
            _logger.Info($"Copying file from \"{source}\" to \"{destination}\".");

            File.Copy(source, destination);
        }

        public void Copy(byte[] fileContent, string destination)
        {
            _logger.Info($"Copying file from memory to \"{destination}\".");

            File.WriteAllBytes(destination, fileContent);
        }
    }
}