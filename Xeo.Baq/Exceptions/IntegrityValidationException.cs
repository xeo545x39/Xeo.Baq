using System;
using System.IO;
using System.Runtime.Serialization;

namespace Xeo.Baq.Exceptions
{
    [Serializable]
    public class IntegrityValidationException : Exception
    {
        public IntegrityValidationException(string message, FileInfo file1, FileInfo file2) : base(message)
        {
            File1 = file1;
            File2 = file2;
        }

        public IntegrityValidationException(string message,
            FileInfo file1,
            FileInfo file2,
            Exception inner) : base(message, inner)
        {
            File1 = file1;
            File2 = file2;
        }

        protected IntegrityValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

        public FileInfo File1 { get; }
        public FileInfo File2 { get; }
    }
}