using System;
using System.IO;
using System.Threading.Tasks;


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

        /*protected override byte[] Hash
        {
            get
            {
                if (hash == null)
                {
                    hash = base.Hash;
                }

                return hash;
            }
        }*/

        private readonly string fullName;
    }
}
