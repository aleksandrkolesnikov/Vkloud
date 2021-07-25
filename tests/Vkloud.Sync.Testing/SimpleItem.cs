using System.IO;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace SyncTests
{
    sealed class SimpleItem : StorageItem
    {
        public SimpleItem(byte[] data) => buffer = data;

        public override Task<Stream> GetContentAsync()
        {
            var stream = new MemoryStream(buffer, false);

            return Task.FromResult(stream as Stream);
        }

        private readonly byte[] buffer;
    }
}
