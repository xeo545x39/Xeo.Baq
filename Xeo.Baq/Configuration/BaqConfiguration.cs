using System.IO;
using Microsoft.Extensions.Configuration;

namespace Xeo.Baq.Configuration
{
    public interface IBaqConfiguration
    {
        IConfiguration Configuration { get; }
        string Path { get; }
        string GetRaw();
    }

    public class BaqConfiguration : IBaqConfiguration
    {
        public BaqConfiguration(IConfiguration configuration, string path)
        {
            Configuration = configuration;
            Path = path;
        }

        public IConfiguration Configuration { get; }
        public string Path { get; }

        public string GetRaw()
            => File.ReadAllText(Path);
    }
}