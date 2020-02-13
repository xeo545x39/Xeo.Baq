using System;
using System.Runtime.Serialization;
using Xeo.Baq.Filtering;

namespace Xeo.Baq.Exceptions
{
    [Serializable]
    public class BackupOperationException : Exception
    {
        public BackupOperationException(string message, Exception inner) : base(message, inner) { }

        public BackupOperationException(string message, BackupEntry entry) : base(message)
            => Entry = entry;

        public BackupOperationException(string message, BackupEntry entry, Exception inner) : base(message, inner)
            => Entry = entry;

        protected BackupOperationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

        public BackupEntry Entry { get; }
    }
}