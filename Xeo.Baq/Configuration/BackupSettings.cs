using System;
using System.Collections.Generic;
using NCrontab;
using Xeo.Baq.Filtering;

namespace Xeo.Baq.Configuration
{
    public class BackupSettings
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public IEnumerable<FileSystemFilter> Filters { get; set; }
        public IEnumerable<FileSystemFilter> Triggers { get; set; }
        public CrontabSchedule Schedule { get; set; }
        public Type BackupType { get; set; }
    }
}