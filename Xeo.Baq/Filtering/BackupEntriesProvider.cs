using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xeo.Baq.Configuration;

namespace Xeo.Baq.Filtering
{
    public interface IBackupEntriesProvider
    {
        IDictionary<BackupEntryKey, BackupEntry> Get(IEnumerable<FileSystemFilter> filters);
    }

    public class BackupEntriesProvider : IBackupEntriesProvider
    {
        private readonly ApplicationSettings _applicationSettings;

        public BackupEntriesProvider(ApplicationSettings applicationSettings)
            => _applicationSettings = applicationSettings;

        public IDictionary<BackupEntryKey, BackupEntry> Get(IEnumerable<FileSystemFilter> filters)
        {
            var backupEntries = new Dictionary<BackupEntryKey, BackupEntry>();

            //foreach (IGrouping<string, FileSystemFilter> filterGroup in filters.GroupBy(x => x.Path))
            foreach (FileSystemFilter filter in filters)
            {
                CheckFilter(filter);

                bool shouldSearchSub = ShouldSearchSub(filter);
                string searchPattern = filter.Regex != null
                    ? "*"
                    : filter.NameMask;

                IEnumerable<string> files = GetFiles(shouldSearchSub, filter, searchPattern);

                if (filter.Regex != null)
                {
                    Regex regex = CreateRegex(filter);

                    files = files.Where(file => regex.IsMatch(file))
                        .ToArray();
                }

                foreach (string file in files)
                {
                    var key = new BackupEntryKey(file);
                    backupEntries.Add(key, new BackupEntry(key));
                }
            }

            return backupEntries;
        }

        private static void CheckFilter(FileSystemFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Path))
            {
                throw new InvalidOperationException("Invalid filter path.");
            }

            if (string.IsNullOrWhiteSpace(filter.NameMask) && string.IsNullOrWhiteSpace(filter.Regex))
            {
                throw new InvalidOperationException(
                    $"Invalid search patterns Both {nameof(filter.Regex)} and {nameof(filter.NameMask)} are null or empty.");
            }
        }

        private static string[] GetFiles(bool shouldSearchSub, FileSystemFilter filter, string searchPattern)
            => shouldSearchSub
                ? Directory.GetFiles(filter.Path, searchPattern, SearchOption.AllDirectories)
                : Directory.GetFiles(filter.Path, searchPattern, SearchOption.TopDirectoryOnly);

        private Regex CreateRegex(FileSystemFilter filter)
            => new Regex(filter.Regex,
                RegexOptions.Singleline | RegexOptions.IgnoreCase,
                TimeSpan.FromSeconds(_applicationSettings.RegexTimeoutSeconds));

        private static bool ShouldSearchSub(FileSystemFilter filter)
            => filter.Path.EndsWith("/*") || filter.Path.EndsWith(@"\*");
    }
}