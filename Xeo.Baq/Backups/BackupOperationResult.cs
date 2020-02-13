using System;
using System.Collections.Generic;
using System.Linq;
using Xeo.Baq.Exceptions;

namespace Xeo.Baq.Backups
{
    public class BackupOperationResult
    {
        public BackupOperationResult(DateTime timeStarted,
            DateTime timeEnded,
            TimeSpan duration,
            IEnumerable<BackupOperationException> exceptions)
        {
            TimeStarted = timeStarted;
            TimeEnded = timeEnded;
            Duration = duration;
            Exceptions = exceptions;
            Success = !exceptions.Any();
        }

        public DateTime TimeStarted { get; }
        public DateTime TimeEnded { get; }
        public TimeSpan Duration { get; }
        public bool Success { get; }
        public IEnumerable<BackupOperationException> Exceptions { get; }
    }
}