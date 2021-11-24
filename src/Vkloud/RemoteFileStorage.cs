using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;
using VkApi.Core;

using System.Diagnostics;


namespace Vkloud
{
    sealed class RemoteFileStorage : IFileStorage
    {
        public event EventHandler<FileStorageEventArgs> Added = delegate { };
        public event EventHandler<FileStorageEventArgs> Removed = delegate { };

        public RemoteFileStorage(string login, string password)
        {
            vkClient = Client.Create(login, password).Result;
            var documents = vkClient.GetDocumentsAsync().Result;
            files = new(documents
                        .Select(doc => new RemoteFile(doc))
                        .Select(file => new KeyValuePair<string, RemoteFile>(file.Path, file)));

            Trace.WriteLine($"RemoteStorage has {files.Count} files");
        }

        public IEnumerable<AbstractFile> Files => files.Values;

        public async Task Add(AbstractFile file)
        {
            using var content = await file.GetContentAsync();
            var memStream = new System.IO.MemoryStream();
            await content.CopyToAsync(memStream);
            var document = await vkClient.UploadDocumentAsync(file.Path.Replace(System.IO.Path.DirectorySeparatorChar, ':'), memStream.ToArray());
            var remoteFile = new RemoteFile(document);
            files[remoteFile.Path] = remoteFile;

            Trace.WriteLine($"{document.Title} has been uploaded");
        }

        public bool Contains(AbstractFile file)
        {
            if (files.TryGetValue(file.Path, out var targetFile))
            {
                return file.Equals(targetFile);
            }

            return false;
        }

        public async Task Remove(AbstractFile file)
        {
            if (files.TryGetValue(file.Path, out var targetFile))
            {
                if (file.Equals(targetFile))
                {
                    await vkClient.RemoveDocumentAsync(targetFile.Document);
                    files.Remove(targetFile.Path);
                }
            }
        }

        private readonly Client vkClient;
        private readonly Dictionary<string, RemoteFile> files;
    }
}
