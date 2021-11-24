using System.IO;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace Vkloud
{
    sealed class LocalFile : AbstractFile
    {
        public LocalFile(string fullName) => this.fullName = fullName;

        public override string Path { get; init; }

        public override Task<Stream> GetContentAsync()
        {
            var stream = File.OpenRead(fullName);

            return Task.FromResult<Stream>(stream);
        }

        private readonly string fullName;
    }
}
