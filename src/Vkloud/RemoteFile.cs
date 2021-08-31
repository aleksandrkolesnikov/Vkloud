using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using VkApi.Core;


namespace Vkloud
{
    sealed class RemoteFile : AbstractFile
    {
        public RemoteFile(Document doc)
        {
            Document = doc;
            Path = doc.Title.Replace(':', System.IO.Path.DirectorySeparatorChar);
            hash = new(() => base.Hash);
        }

        public Document Document { get; }

        public override string Path { get; init; }

        public override async Task<Stream> GetContentAsync()
        {
            //TODO: should cache data. E.g. store it localy

            var request = WebRequest.CreateHttp(Document.Url);
            var response = await request.GetResponseAsync();
            var stream = response.GetResponseStream();

            return stream;
        }

        public override byte[] Hash => Document.Hash ?? hash.Value;

        private readonly Lazy<byte[]> hash;
    }
}
