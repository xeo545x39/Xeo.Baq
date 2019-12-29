using System;
using System.Collections.Generic;
using Xeo.Baq.Backups;
using Xeo.Baq.Configuration;

namespace Xeo.Baq
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


        public BackupManager(IEnumerable<BackupSettings> backupSettingsList,
            IBackupOperationResultProvider backupOperationResultProvider,
            Func<BackupSettings, FullBackupPerformer> fullBackupPerformerFactory)
        {
            _backupSettingsList = backupSettingsList;
            _fullBackupPerformerFactory = fullBackupPerformerFactory;

            OperationResultProvider = backupOperationResultProvider;
        }

        public IBackupOperationResultProvider OperationResultProvider { get; }

        public void RunBackups()
        {
            foreach (BackupSettings settings in _backupSettingsList)
            {
                Func<BackupSettings, IBackupPerformer> factory = GetBackupPerformerFactory(settings.BackupType);

                IBackupPerformer backupPerformer = factory(settings);

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