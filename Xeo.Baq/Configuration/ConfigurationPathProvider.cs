namespace Xeo.Baq.Configuration
{
    public interface IConfigurationPathProvider
    {
        string Get();
    }


    public class ConfigurationPathProvider : IConfigurationPathProvider
    {
        public string Get()
            => "appsettings.json";
    }
}