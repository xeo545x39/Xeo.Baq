using System;
using System.Collections.Generic;

namespace Xeo.Baq.Backups
{
    public class DummyBackupOperationResultProvider : IBackupOperationResultProvider
    {
        public IEnumerable<BackupOperationResult> GetResults(Guid backupId)
        {
            DateTime startTime = DateTime.Now.AddDays(-2);
            DateTime endTime = startTime.AddMinutes(0);

            return new[] { new BackupOperationResult(startTime, endTime, true) };
        }

        public IEnumerable<BackupOperationResult> GetResults<TBackup>()
            => throw new NotImplementedException();
    }
}