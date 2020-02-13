using System;
using Microsoft.Extensions.Configuration;

namespace Xeo.Baq.Configuration.Extensions
{
    public static class IConfigurationExtensions
    {
        private const string SectionRemovablePostfix = "Settings";

        public static T GetSettings<T>(this IConfiguration @this, string sectionKey = null)
            where T : new()
        {
            var settings = new T();
            @this.BindTo(settings, sectionKey);

            return settings;
        }

        public static void BindTo<T>(this IConfiguration @this, T instance, string sectionKey = null)
        {
            sectionKey ??= typeof(T).Name.Replace(SectionRemovablePostfix, string.Empty);

            if (string.IsNullOrEmpty(sectionKey))
            {
                throw new ArgumentException("Invalid configuration section key.", nameof(sectionKey));
            }

            IConfigurationSection section = @this.GetSection(sectionKey);
            section.Bind(instance);
        }
    }
}