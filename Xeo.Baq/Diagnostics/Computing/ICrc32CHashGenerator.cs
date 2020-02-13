using Force.Crc32;

namespace Xeo.Baq.Diagnostics.Computing
{
    public class Crc32CHashGenerator : IHashGenerator
    {
        public string AlgorithmName => "CRC32C";

        public int Generate(byte[] data, int offset, int length)
            => (int) Crc32CAlgorithm.Compute(data, offset, length);

        public int Generate(byte[] data)
            => (int) Crc32CAlgorithm.Compute(data, 0, data.Length);
    }
}