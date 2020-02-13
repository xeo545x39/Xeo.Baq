using System;
using Xeo.Baq.Backups.Performers;
using Xeo.Baq.Backups.Types;
using Xeo.Baq.Configuration;

namespace Xeo.Baq.Backups.Factories
{
    public interface IBackupPerformerFactory
    {
        Func<BackupSettings, IBackupPerformer> GetFactoryByBackupType(IBackupType backupType);
    }

    public class BackupPerformerFactory : IBackupPerformerFactory
    {
        private readonly Func<BackupSettings, IFullBackupPerformer> _fullBackupPerformer;

        public BackupPerformerFactory(Func<BackupSettings, IFullBackupPerformer> fullBackupPerformer)
            => _fullBackupPerformer = fullBackupPerformer;

        public Func<BackupSettings, IBackupPerformer> GetFactoryByBackupType(IBackupType backupType)
        {
            if (backupType is FullBackup)
            {
                return _fullBackupPerformer;
            }

            throw new InvalidOperationException($"There is no backup performer for backup type \"{backupType.GetType().FullName}\".");
        }
    }
}