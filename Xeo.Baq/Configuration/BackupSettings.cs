using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NCrontab;
using Xeo.Baq.Configuration.Serialization.Json;
using Xeo.Baq.Filtering;

namespace Xeo.Baq.Configuration
{
    public class BackupSettings
    {
        public BackupSettings()
            => Filters = new List<FileSystemFilter>();

        //Triggers = new List<FileSystemFilter>();
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public bool CheckIntegrity { get; set; }

        public IEnumerable<FileSystemFilter> Filters { get; set; }
        //public IEnumerable<FileSystemFilter> Triggers { get; set; }

        [JsonConverter(typeof(JsonCronConverter))]
        public CrontabSchedule Schedule { get; set; }

        [JsonConverter(typeof(JsonTypeConverter))]
        public Type BackupType { get; set; }
    }
}