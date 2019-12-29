using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Xeo.Baq.Configuration;

namespace Xeo.Baq.Backups
{
    public interface IBackupManager
    {
        void RunBackups();
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

        public void RunBackups()
        {
            foreach (BackupSettings settings in _backupSettingsList)
            {
                Func<BackupSettings, IBackupPerformer> factory = GetBackupPerformerFactory(settings.BackupType);

                IBackupPerformer backupPerformer = factory(settings);

                _logger.Info($"Performing backup with Id = '{settings.Id}'.");
                
                backupPerformer.Perform();
            }
        }

        public void RunBackup(Guid id)
        {
            throw new NotImplementedException();
        }

        private Func<BackupSettings, IBackupPerformer> GetBackupPerformerFactory(Type backupType)
        {
            if (backupType == typeof(FullBackupPerformer))
            {
                return _fullBackupPerformerFactory;
            }

            throw new InvalidOperationException($"Cannot provide backup performer factory for backup type '{backupType.FullName}'");
        }
    }
}