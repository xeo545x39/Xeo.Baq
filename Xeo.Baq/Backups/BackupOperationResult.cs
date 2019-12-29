using System;

namespace Xeo.Baq.Backups
{
    public class BackupOperationResult
    {
        public BackupOperationResult(DateTime timeStarted, DateTime timeEnded, bool success)
        {
            TimeStarted = timeStarted;
            TimeEnded = timeEnded;
            Success = success;
        }

        public DateTime TimeStarted { get; set; }
        public DateTime TimeEnded { get; set; }
        public bool Success { get; set; }
    }
}