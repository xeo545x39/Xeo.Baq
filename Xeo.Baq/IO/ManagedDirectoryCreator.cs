using System.IO;
using NLog;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.IO
{
    public class ManagedDirectoryCreator : IDirectoryCreator
    {
        private readonly ILogger _logger;

        public ManagedDirectoryCreator(ILogger logger)
            => _logger = logger;

        public void Create(string destination)
        {
            if (!Directory.Exists(destination))
            {
                _logger.Info($"Creating directory \"{destination}\".");

                Directory.CreateDirectory(destination);
            }
        }
    }
}