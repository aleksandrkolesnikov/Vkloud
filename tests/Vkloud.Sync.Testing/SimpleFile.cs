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

        public override byte[] Hash
        {
            get
            {
                if (hash == null)
                {
                    using var sha256 = SHA256.Create();
                    using var content = GetContentAsync().Result;

                    hash = sha256.ComputeHash(content);
                }

                return hash;
            }
        }

        public override Task<Stream> GetContentAsync()
        {
            var stream = new MemoryStream(buffer, false);

            return Task.FromResult(stream as Stream);
        }

        private readonly byte[] buffer;

        private byte[] hash;
    }
}
