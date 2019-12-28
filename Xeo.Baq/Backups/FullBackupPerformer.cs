using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeo.Baq.Configuration;
using Xeo.Baq.Extensions;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups
{
    public class FullBackupPerformer : IBackupPerformer
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IBackupEntriesProvider _backupEntriesProvider;
        private readonly IBackupOperationResultProvider _backupOperationResultProvider;
        private readonly BackupSettings _backupSettings;
        private readonly IFileCopier _fileCopier;
        private readonly IParallelFileSystemManipulator _parallelFileSystemManipulator;

        public FullBackupPerformer(BackupSettings backupSettings,
            IBackupEntriesProvider backupEntriesProvider,
            IFileCopier fileCopier,
            IParallelFileSystemManipulator parallelFileSystemManipulator,
            IBackupOperationResultProvider backupOperationResultProvider,
            ApplicationSettings applicationSettings)
        {
            _backupSettings = backupSettings;
            _backupEntriesProvider = backupEntriesProvider;
            _fileCopier = fileCopier;
            _parallelFileSystemManipulator = parallelFileSystemManipulator;
            _backupOperationResultProvider = backupOperationResultProvider;
            _applicationSettings = applicationSettings;
        }

        public void Perform(bool force = false)
        {
            DateTime? lastTimeEnded = _backupOperationResultProvider.GetResults(_backupSettings.Id)
                .OrderByDescending(x => x.TimeEnded)
                .FirstOrDefault()
                ?.TimeEnded;

            if (lastTimeEnded == null ||
                _backupSettings.Schedule.GetNextOccurrences(lastTimeEnded.Value, DateTime.Now)
                    .Any())
            {
                IDictionary<BackupEntryKey, BackupEntry> backupEntries = _backupEntriesProvider.Get(_backupSettings.Filters);

                foreach (IEnumerable<KeyValuePair<BackupEntryKey, BackupEntry>> batchedEntries in backupEntries.Batch(_applicationSettings
                    .ParallelOperationMaxItems))
                {
                    IFileSystemActionParameter[] actionParams = batchedEntries.Select(x
                            => (IFileSystemActionParameter) new GenericDestinationParameter
                            {
                                Destination = GetAbsoluteDestination(x.Key.Path, _backupSettings.Destination),
                                Source = x.Value
                            })
                        .ToArray();

                    _parallelFileSystemManipulator.Do(param =>
                        {
                            var castedParam = (GenericDestinationParameter) param;

                            _fileCopier.Copy(param.Source.ToString(), castedParam.Destination);
                        },
                        actionParams);
                }
            }
        }

        private static string GetAbsoluteDestination(string source, string destination)
        {
            string sourceRelative = source.Replace(Path.GetPathRoot(source), string.Empty);

            return Path.Combine(destination, sourceRelative);
        }
    }
}