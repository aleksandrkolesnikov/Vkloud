using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Vkloud.Sync;


namespace SyncTests
{
    sealed class SimpleFile : AbstractFile
    {
        public SimpleFile(byte[] data, string path)
        {
            buffer = data;
            Path = path;
        }

        public override string Path { get; init; }

        public override Task<Stream> GetContentAsync()
        {
            var stream = new MemoryStream(buffer, false);

            return Task.FromResult(stream as Stream);
        }

        private readonly byte[] buffer;
    }
}
