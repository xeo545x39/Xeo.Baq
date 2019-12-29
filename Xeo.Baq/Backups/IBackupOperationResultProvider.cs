using System;
using System.Collections.Generic;

namespace Xeo.Baq
{
    public interface IBackupOperationResultProvider
    {
        IEnumerable<BackupOperationResult> GetResults(Guid backupId);
        IEnumerable<BackupOperationResult> GetResults<TBackup>();
    }
}