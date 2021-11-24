using System.IO;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace Vkloud
{
    sealed class BarelyLocalFile : AbstractFile
    {
        public BarelyLocalFile(AbstractFile file, string fullPath)
        {
            this.file = file;
            this.fullPath = fullPath;
        }

        public override string Path { get => file.Path; init { } }

        public override Task<Stream> GetContentAsync() => file.GetContentAsync();

        /*public async Task<LocalFile> ToLocalFile()
        {
            using (var fileStream = File.OpenWrite(fullPath))
            using (var content = await file.GetContentAsync())
            {
                await content.CopyToAsync(fileStream);
            }

            var localFile = new LocalFile(fullPath) { Path = file.Path };
            return localFile;
        }*/

        private readonly AbstractFile file;
        private readonly string fullPath;
    }
}
