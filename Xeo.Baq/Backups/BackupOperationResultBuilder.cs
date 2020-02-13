using System;
using System.Collections.Concurrent;
using Xeo.Baq.Exceptions;
using Xeo.Baq.Extensions;

namespace Xeo.Baq.Backups
{
    public interface IBackupOperationResultBuilder
    {
        IBackupOperationResultBuilder AddException(BackupOperationException exception);
        IBackupOperationResultBuilder SetDuration(TimeSpan duration);
        IBackupOperationResultBuilder SetStartTime(DateTime start);
        IBackupOperationResultBuilder SetEndTime(DateTime end);
        BackupOperationResult Build();
    }

    public class BackupOperationResultBuilder : IBackupOperationResultBuilder
    {
        private readonly ConcurrentBag<BackupOperationException> _exceptions;
        private TimeSpan _duration;
        private DateTime _end;
        private DateTime _start;

        public BackupOperationResultBuilder()
            => _exceptions = new ConcurrentBag<BackupOperationException>();

        public IBackupOperationResultBuilder AddException(BackupOperationException exception)
            => this.ReturnThis(() => _exceptions.Add(exception));

        public IBackupOperationResultBuilder SetDuration(TimeSpan duration)
            => this.ReturnThis(() => _duration = duration);

        public IBackupOperationResultBuilder SetStartTime(DateTime start)
            => this.ReturnThis(() => _end = start);

        public IBackupOperationResultBuilder SetEndTime(DateTime end)
            => this.ReturnThis(() => _start = end);

        public BackupOperationResult Build()
            => new BackupOperationResult(_end,
                _start,
                _duration,
                _exceptions);
    }
}