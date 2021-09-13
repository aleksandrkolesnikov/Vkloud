using System;
using System.IO;
using System.Threading.Tasks;


namespace Vkloud
{
    // Carry memories of real local file
    sealed class FakeFile : AbstractFile
    {
        public FakeFile(string path, byte[] hash)
        {
            Path = path;
            Hash = hash;
        }

        public override string Path { get; init; }

        public override byte[] Hash { get; }

        public override Task<Stream> GetContentAsync() => throw new NotSupportedException();
    }
}
