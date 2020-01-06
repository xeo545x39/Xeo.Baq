using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeo.Baq.Configuration;
using Xeo.Baq.Exceptions;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups
{
    public interface IFileSystemActionExecutor
    {
        void Execute(IEnumerable<BackupEntry> entries, BackupSettings backupSettings);
    }

    public class FileSystemActionExecutor : IFileSystemActionExecutor
    {
        private readonly IDirectoryCreator _directoryCreator;
        private readonly IFileCopier _fileCopier;
        private readonly Func<IGenericExceptionHandler> _genericExceptionHandlerFactory;
        private readonly IParallelFileSystemManipulator _parallelFileSystemManipulator;

        public FileSystemActionExecutor(IParallelFileSystemManipulator parallelFileSystemManipulator,
            IFileCopier fileCopier,
            IDirectoryCreator directoryCreator,
            Func<IGenericExceptionHandler> genericExceptionHandlerFactory)
        {
            _parallelFileSystemManipulator = parallelFileSystemManipulator;
            _fileCopier = fileCopier;
            _directoryCreator = directoryCreator;
            _genericExceptionHandlerFactory = genericExceptionHandlerFactory;
        }

        public void Execute(IEnumerable<BackupEntry> entries, BackupSettings backupSettings)
        {
            IFileSystemActionParameter[] actionParams = entries
                .Select(entry => GetParameter(entry, backupSettings))
                .ToArray();

            _parallelFileSystemManipulator.Do(param =>
                {
                    if (param is CopyFileActionParameter copyFileParameter)
                    {
                        string destinationDirectory = Path.GetDirectoryName(copyFileParameter.Destination);

                        IGenericExceptionHandler exceptionHandler = _genericExceptionHandlerFactory();

                        exceptionHandler
                            .Handle(() => _directoryCreator.Create(destinationDirectory))
                            .ContinueOnSuccess(() => _fileCopier.Copy(copyFileParameter.Source.ToString(), copyFileParameter.Destination));
                    }
                    else if (param is CreateDirectoryActionParameter createDirectoryParam)
                    {
                        _directoryCreator.Create(createDirectoryParam.Destination);
                    }
                },
                actionParams);
        }

        private IFileSystemActionParameter GetParameter(BackupEntry entry, BackupSettings backupSettings)
        {
            if (entry.EntryType == FileSystemEntryType.Directory)
            {
                return new CreateDirectoryActionParameter
                {
                    Destination = GetAbsoluteDestination(entry.Key.Path, backupSettings.Destination)
                };
            }

            return new CopyFileActionParameter
            {
                Destination = GetAbsoluteDestination(entry.Key.Path, backupSettings.Destination),
                Source = entry
            };
        }

        private static string GetAbsoluteDestination(string source, string destination)
        {
            string sourceRelative = source.Replace(Path.GetPathRoot(source), string.Empty);

            return Path.Combine(destination, sourceRelative);
        }
    }
}