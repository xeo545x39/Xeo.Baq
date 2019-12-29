using System;
using System.Collections.Generic;

namespace Xeo.Baq.Backups
{
    public interface IBackupOperationResultProvider
    {
        IEnumerable<BackupOperationResult> GetResults(Guid backupId);
        IEnumerable<BackupOperationResult> GetResults<TBackup>();
    }
}