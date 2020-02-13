using System;
using System.Collections.Generic;
using Xeo.Baq.Exceptions;

namespace Xeo.Baq.Backups
{
    public class DummyBackupOperationResultProvider : IBackupOperationResultProvider
    {
        public IEnumerable<BackupOperationResult> GetResults(Guid backupId)
        {
            DateTime startTime = DateTime.Now.AddDays(-2);
            DateTime endTime = startTime.AddMinutes(2);

            return new[]
            {
                new BackupOperationResult(startTime,
                    endTime,
                    endTime - startTime,
                    new BackupOperationException[]
                        { })
            };
        }

        public IEnumerable<BackupOperationResult> GetResults<TBackup>()
            => throw new NotImplementedException();
    }
}