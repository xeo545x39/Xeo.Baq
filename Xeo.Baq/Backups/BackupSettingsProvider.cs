using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Xeo.Baq.Configuration;

namespace Xeo.Baq.Backups
{
    public interface IBackupSettingsProvider
    {
        IEnumerable<BackupSettings> Load();
    }

    public class BackupSettingsProvider : IBackupSettingsProvider
    {
        private const string SectionName = "Backups";

        private readonly IBaqConfiguration _configuration;

        public BackupSettingsProvider(IBaqConfiguration configuration)
            => _configuration = configuration;

        public IEnumerable<BackupSettings> Load()
        {
            ICollection<BackupSettings> backups = new List<BackupSettings>();
            JObject jObject = JObject.Parse(_configuration.GetRaw());
            var backupsArray = (JArray) jObject[SectionName];

            foreach (JObject x in backupsArray)
            {
                var itemJson = x.ToString();
                var settings = JsonSerializer.Deserialize<BackupSettings>(itemJson);

                backups.Add(settings);
            }

            return backups;
        }
    }
}