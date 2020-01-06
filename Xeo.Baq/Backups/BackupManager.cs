using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Xeo.Baq.Backups.Performers;
using Xeo.Baq.Configuration;
using Xeo.Baq.Diagnostics;

namespace Xeo.Baq.Backups
{
    public interface IBackupManager
    {
        void RunAllBackups();
        void RunBackup(Guid id);
    }

    public class BackupManager : IBackupManager
    {
        private readonly IEnumerable<BackupSettings> _backupSettingsList;
        private readonly Func<BackupSettings, FullBackupPerformer> _fullBackupPerformerFactory;
        private readonly ILogger _logger;

        public BackupManager(IEnumerable<BackupSettings> backupSettingsList,
            IBackupOperationResultProvider backupOperationResultProvider,
            Func<BackupSettings, FullBackupPerformer> fullBackupPerformerFactory,
            ILogger logger)
        {
            _backupSettingsList = backupSettingsList;
            _fullBackupPerformerFactory = fullBackupPerformerFactory;
            _logger = logger;

            OperationResultProvider = backupOperationResultProvider;
        }

        public IBackupOperationResultProvider OperationResultProvider { get; }

        public void RunAllBackups()
        {
            foreach (BackupSettings settings in _backupSettingsList)
            {
                RunInternal(settings);
            }
        }

        public void RunBackup(Guid id)
        {
            BackupSettings settings = _backupSettingsList.SingleOrDefault(x => x.Id == id);
            if (settings == null)
            {
                throw new InvalidOperationException($"There is no backup with Id = \"{id}\".");
            }

            RunInternal(settings);
        }

        private void RunInternal(BackupSettings settings)
        {
            Func<BackupSettings, IBackupPerformer> factory = GetBackupPerformerFactory(settings.BackupType);
            IBackupPerformer performer = factory(settings);
            TimeSpan backupDuration = default;

            _logger.Info($"Started performing backup with Id = \"{settings.Id}\".");

            try
            {
                Measured.Go(() => performer.Perform(), out backupDuration);
            }
            catch (Exception e)
            {
                _logger.Info($"Error occurred while performing backup with Id = \"{settings.Id}\".");
                throw;
            }
            finally
            {
                _logger.Info($"Finished performing backup with Id = \"{settings.Id}\". Duration: {backupDuration}");
            }
        }

        private Func<BackupSettings, IBackupPerformer> GetBackupPerformerFactory(Type backupType)
        {
            if (backupType == typeof(FullBackupPerformer))
            {
                return _fullBackupPerformerFactory;
            }

            throw new InvalidOperationException($"Cannot provide backup performer factory for backup type \"{backupType.FullName}\".");
        }
    }
}