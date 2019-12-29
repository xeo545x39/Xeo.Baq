using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xeo.Baq.Configuration;
using Xeo.Baq.Extensions;

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

            foreach (FileSystemFilter filter in filters)
            {
                CheckFilter(filter);

                bool shouldSearchSub = ShouldSearchSub(filter);
                string searchPattern = GetSearchPattern(filter);

                PrepareFilter(filter);

                HandleFile(filter,
                    shouldSearchSub,
                    searchPattern,
                    backupEntries);

                HandleDirectory(filter,
                    shouldSearchSub,
                    searchPattern,
                    backupEntries);
            }

            return backupEntries;
        }

        private void HandleDirectory(FileSystemFilter filter,
            bool shouldSearchSub,
            string searchPattern,
            Dictionary<BackupEntryKey, BackupEntry> backupEntries)
        {
            if (filter.EntryType.HasFlag(FileSystemEntryType.Directory))
            {
                IEnumerable<string> directories = GetDirectories(shouldSearchSub, filter, searchPattern);

                foreach (string directory in directories)
                {
                    var key = new BackupEntryKey(directory);
                    backupEntries.Add(key, new BackupEntry(key, FileSystemEntryType.Directory));
                }
            }
        }

        private void HandleFile(FileSystemFilter filter,
            bool shouldSearchSub,
            string searchPattern,
            Dictionary<BackupEntryKey, BackupEntry> backupEntries)
        {
            if (filter.EntryType.HasFlag(FileSystemEntryType.File))
            {
                IEnumerable<string> files = GetFiles(shouldSearchSub, filter, searchPattern);

                foreach (string file in files)
                {
                    var key = new BackupEntryKey(file);
                    backupEntries.Add(key, new BackupEntry(key, FileSystemEntryType.File));
                }
            }
        }

        private static string GetSearchPattern(FileSystemFilter filter)
            => filter.Regex != null
                ? "*"
                : filter.NameMask;

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

        private string[] GetFiles(bool shouldSearchSub, FileSystemFilter filter, string searchPattern)
        {
            string[] files = shouldSearchSub
                ? Directory.GetFiles(filter.Path, searchPattern, SearchOption.AllDirectories)
                : Directory.GetFiles(filter.Path, searchPattern, SearchOption.TopDirectoryOnly);

            IEnumerable<string> output = files
                .Select(f => new FileInfo(f))
                .Where(f => filter.Attributes.Any(a => f.Attributes.HasFlag(a)))
                .ConditionalWhere(
                    f => filter.Regex != null,
                    () => CreateRegex(filter),
                    (file, regex) => regex.IsMatch(file.FullName))
                .Select(f => f.FullName);

            return output.ToArray();
        }

        private string[] GetDirectories(bool shouldSearchSub, FileSystemFilter filter, string searchPattern)
        {
            string[] directories = shouldSearchSub
                ? Directory.GetDirectories(filter.Path, searchPattern, SearchOption.AllDirectories)
                : Directory.GetDirectories(filter.Path, searchPattern, SearchOption.TopDirectoryOnly);

            IEnumerable<string> output = directories
                .Select(d => new DirectoryInfo(d))
                .Where(d => filter.Attributes.Any(a => d.Attributes.HasFlag(a)))
                .ConditionalWhere(x => filter.Regex != null,
                    () => CreateRegex(filter),
                    (directory, regex) => regex.IsMatch(directory.FullName))
                .Select(d => d.FullName);

            return output.ToArray();
        }

        private void PrepareFilter(FileSystemFilter filter)
        {
            filter.Path = filter.Path.TrimEnd('*');
        }

        private Regex CreateRegex(FileSystemFilter filter)
            => new Regex(filter.Regex,
                RegexOptions.Singleline | RegexOptions.IgnoreCase,
                TimeSpan.FromSeconds(_applicationSettings.RegexTimeoutSeconds));

        private static bool ShouldSearchSub(FileSystemFilter filter)
            => filter.Path.EndsWith("/*") || filter.Path.EndsWith(@"\*");
    }
}