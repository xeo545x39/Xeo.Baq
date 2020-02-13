using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Xeo.Baq.Configuration;
using Xeo.Baq.Extensions;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Performers
{
    public interface IFullBackupPerformer : IBackupPerformer { }

    public class FullBackupPerformer : IFullBackupPerformer
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IBackupEntriesProvider _backupEntriesProvider;
        private readonly Lazy<IBackupOperationResultProvider> _backupOperationResultProviderLazy;
        private readonly BackupSettings _backupSettings;
        private readonly IFileSystemActionExecutor _fileSystemActionExecutor;
        private readonly ILifetimeScope _scope;

        public FullBackupPerformer(BackupSettings backupSettings,
            IBackupEntriesProvider backupEntriesProvider,
            Lazy<IBackupOperationResultProvider> backupOperationResultProviderLazy,
            IFileSystemActionExecutor fileSystemActionExecutor,
            ApplicationSettings applicationSettings,
            ILifetimeScope scope)
        {
            _backupSettings = backupSettings;
            _backupEntriesProvider = backupEntriesProvider;
            _backupOperationResultProviderLazy = backupOperationResultProviderLazy;
            _fileSystemActionExecutor = fileSystemActionExecutor;
            _applicationSettings = applicationSettings;
            _scope = scope;
        }

        public void Perform(bool force = false)
        {
            // TODO extract 
            IBackupOperationResultProvider backupOperationResultProvider = _backupOperationResultProviderLazy.Value;

            DateTime? lastTimeEnded = backupOperationResultProvider.GetResults(_backupSettings.Id)
                .OrderByDescending(x => x.TimeEnded)
                .FirstOrDefault()
                ?.TimeEnded;

            if (lastTimeEnded == null ||
                    _backupSettings.Schedule.GetNextOccurrences(lastTimeEnded.Value, DateTime.Now)
                        .Any())
                // /TODO
            {
                IDictionary<BackupEntryKey, BackupEntry> backupEntries = _backupEntriesProvider.Get(_backupSettings.Filters);

                foreach (IEnumerable<KeyValuePair<BackupEntryKey, BackupEntry>> batchedEntries in backupEntries.Batch(_applicationSettings
                    .ParallelOperationMaxItems))
                {
                    _fileSystemActionExecutor.Execute(
                        batchedEntries.Select(x => x.Value),
                        _backupSettings);
                }
            }
        }
    }
}