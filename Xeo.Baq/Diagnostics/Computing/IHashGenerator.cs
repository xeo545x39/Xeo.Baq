namespace Xeo.Baq.Diagnostics.Computing
{
    public interface IHashGenerator
    {
        string AlgorithmName { get; }

        int Generate(byte[] data, int offset, int length);
        int Generate(byte[] data);
    }
}