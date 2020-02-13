using Xeo.Baq.Exceptions;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.Backups.Actions
{
    public interface ICreateDirectoryAction : IFileSystemAction { }

    public class CreateDirectoryAction : ICreateDirectoryAction
    {
        private readonly IDirectoryCreator _directoryCreator;

        public CreateDirectoryAction(IDirectoryCreator directoryCreator)
            => _directoryCreator = directoryCreator;

        public void Execute(IFileSystemActionParameter parameter, IExceptionHandlerAction exceptionHandlerAction)
        {
            var castedParameter = (CreateDirectoryActionParameter) parameter;

            exceptionHandlerAction.Describe($"Directory creation \"{castedParameter.Destination}\".");

            _directoryCreator.Create(castedParameter.Destination);
        }
    }
}