using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeo.Baq.Backups;
using Xeo.Baq.Backups.Actions;
using Xeo.Baq.Configuration;
using Xeo.Baq.Exceptions;
using Xeo.Baq.Extensions;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.IO
{
    public interface IFileSystemActionExecutor
    {
        void Execute(IEnumerable<BackupEntry> entries, BackupSettings backupSettings);
    }

    public class FileSystemActionExecutor : IFileSystemActionExecutor
    {
        private readonly IBackupOperationResultBuilder _backupOperationResultBuilder;
        private readonly ICheckIntegrityAction _checkIntegrityAction;
        private readonly ICopyFileAction _copyFileAction;
        private readonly ICreateDirectoryAction _createDirectoryAction;
        private readonly Func<IGenericExceptionHandler> _genericExceptionHandlerFactory;
        private readonly IParallelFileSystemManipulator _parallelFileSystemManipulator;

        public FileSystemActionExecutor(IParallelFileSystemManipulator parallelFileSystemManipulator,
            Func<IGenericExceptionHandler> genericExceptionHandlerFactory,
            IBackupOperationResultBuilder backupOperationResultBuilder,
            ICreateDirectoryAction createDirectoryAction,
            ICopyFileAction copyFileAction,
            ICheckIntegrityAction checkIntegrityAction)
        {
            _parallelFileSystemManipulator = parallelFileSystemManipulator;
            _genericExceptionHandlerFactory = genericExceptionHandlerFactory;
            _backupOperationResultBuilder = backupOperationResultBuilder;
            _createDirectoryAction = createDirectoryAction;
            _copyFileAction = copyFileAction;
            _checkIntegrityAction = checkIntegrityAction;
        }

        public void Execute(IEnumerable<BackupEntry> entries, BackupSettings backupSettings)
        {
            IFileSystemActionParameter[] actionParams = entries
                .Select(entry => GetParameter(entry, backupSettings))
                .ToArray();

            _parallelFileSystemManipulator.Do(param =>
                {
                    IGenericExceptionHandler exceptionHandler = _genericExceptionHandlerFactory();

                    if (param is CopyFileActionParameter copyFileParameter)
                    {
                        MemoryFile sourceFile = MemoryFile.Load(copyFileParameter.Source.ToString());

                        exceptionHandler
                            .Handle(action => _createDirectoryAction.Execute(new CreateDirectoryActionParameter(
                                    Path.GetDirectoryName(copyFileParameter.Destination)),
                                action))
                            .ContinueOnSuccess(action => _copyFileAction.Execute(param, action))
                            .ContinueOnSuccessWhen(() => backupSettings.CheckIntegrity,
                                action => _checkIntegrityAction.Execute(new CheckIntegrityActionParameter(sourceFile, copyFileParameter.Destination), action));
                    }
                    else if (param is CreateDirectoryActionParameter createDirectoryParam)
                    {
                        exceptionHandler.Handle(action => _createDirectoryAction.Execute(param, action));
                    }

                    exceptionHandler.GetCatchedExceptions()
                        .Select(ex => ex is BackupOperationException exception
                            ? exception
                            : new BackupOperationException("Error occured during the backup operation.", ex))
                        .ForEach(ex => _backupOperationResultBuilder.AddException(ex));
                },
                actionParams);
        }

        private IFileSystemActionParameter GetParameter(BackupEntry entry, BackupSettings backupSettings)
        {
            string absoluteDestination = GetAbsoluteDestination(entry.Path, backupSettings.Destination);

            if (entry is DirectoryBackupEntry directoryEntry)
            {
                return new CreateDirectoryActionParameter(absoluteDestination);
            }

            if (entry is FileBackupEntry fileEntry)
            {
                return new CopyFileActionParameter(absoluteDestination, fileEntry);
            }

            throw new NotSupportedException($"Backup entry type \"{entry.GetType().FullName}\" is not supported.");
        }

        private static string GetAbsoluteDestination(string source, string destination)
        {
            string sourceRelative = source.Replace(Path.GetPathRoot(source), string.Empty);

            return Path.Combine(destination, sourceRelative);
        }
    }
}