using Xeo.Baq.Exceptions;
using Xeo.Baq.IO;

namespace Xeo.Baq.Backups.Actions
{
    public interface IFileSystemAction
    {
        void Execute(IFileSystemActionParameter parameter, IExceptionHandlerAction exceptionHandlerAction);
    }
}