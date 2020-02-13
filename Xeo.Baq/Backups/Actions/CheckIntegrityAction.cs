using System.IO;
using Xeo.Baq.Diagnostics.Computing;
using Xeo.Baq.Exceptions;
using Xeo.Baq.IO;
using Xeo.Baq.IO.Abstract;

namespace Xeo.Baq.Backups.Actions
{
    public interface ICheckIntegrityAction : IFileSystemAction { }

    public class CheckIntegrityAction : ICheckIntegrityAction
    {
        private readonly IHashGenerator _hashGenerator;

        public CheckIntegrityAction(IHashGenerator hashGenerator)
            => _hashGenerator = hashGenerator;

        public void Execute(IFileSystemActionParameter parameter, IExceptionHandlerAction exceptionHandlerAction)
        {
            var castedParameter = (CheckIntegrityActionParameter) parameter;

            exceptionHandlerAction.Describe($"Hash computing for file \"{Path.GetFileName(castedParameter.File1.Name)}\".");

            MemoryFile destinationFile = castedParameter.LoadedFile1 ?? MemoryFile.Load(castedParameter.File1.FullName);
            MemoryFile sourceFile = MemoryFile.Load(castedParameter.File2.FullName);

            int sourceHash = _hashGenerator.Generate(sourceFile.Data);
            int destinationHash = _hashGenerator.Generate(destinationFile.Data);

            if (destinationHash != sourceHash)
            {
                throw new IntegrityValidationException(
                    $"The destination file hash ({destinationHash}) is not equal to the source file hash ({sourceHash}).",
                    castedParameter.File1,
                    castedParameter.File2);
            }
        }
    }
}