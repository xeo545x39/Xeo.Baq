using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Xeo.Baq.Configuration.Serialization.Json
{
    public static class JsonHelper
    {
        public static JToken FromConfiguration(IConfiguration config)
        {
            var obj = new JObject();
            foreach (IConfigurationSection child in config.GetChildren())
            {
                obj.Add(child.Key, FromConfiguration(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
            {
                return new JValue(section.Value);
            }

            return obj;
        }
    }
}