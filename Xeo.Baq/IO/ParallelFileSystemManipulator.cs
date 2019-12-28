using System;
using System.Threading.Tasks;

namespace Xeo.Baq.IO
{
    public interface IParallelFileSystemManipulator
    {
        void Do(Action<IFileSystemActionParameter> action, params IFileSystemActionParameter[] parameters);
    }

    public class ParallelFileSystemManipulator : IParallelFileSystemManipulator
    {
        public void Do(Action<IFileSystemActionParameter> action, params IFileSystemActionParameter[] parameters)
        {
            Parallel.ForEach(parameters, (param, state) => action(param));
        }
    }
}