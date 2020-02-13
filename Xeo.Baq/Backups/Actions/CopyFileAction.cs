using Xeo.Baq.Exceptions;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Actions
{
    public interface ICopyFileAction : IFileSystemAction { }

    public class CopyFileAction : ICopyFileAction
    {
        private readonly IFileCopier _fileCopier;

        public CopyFileAction(IFileCopier fileCopier)
            => _fileCopier = fileCopier;

        public void Execute(IFileSystemActionParameter parameter, IExceptionHandlerAction exceptionHandlerAction)
        {
            var castedParameter = (CopyFileActionParameter) parameter;
            string sourcePath = castedParameter.Source.Path;

            exceptionHandlerAction.Describe($"File copying from \"{sourcePath}\" to \"{castedParameter.Destination}\".");

            _fileCopier.Copy(sourcePath, castedParameter.Destination);
        }
    }
}