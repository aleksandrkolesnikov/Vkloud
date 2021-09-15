using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;
using VkApi.Core;

using System.Diagnostics;


namespace Vkloud
{
    sealed class RemoteFileStorage : IStorage
    {
        public event EventHandler<StorageEventArgs> Added;
        public event EventHandler<string> Changed;
        public event EventHandler<StorageEventArgs> Removed;

        public RemoteFileStorage(string login, string password)
        {
            vkClient = new(login, password);
            var documents = vkClient.GetDocumentsAsync().Result;
            files = new(documents
                        .Select(doc => new RemoteFile(doc))
                        .Select(file => new KeyValuePair<string, RemoteFile>(file.Path, file)));

            Trace.WriteLine($"RemoteStorage has {files.Count} files");
        }

        public IEnumerable<StorageItem> Items
        {
            get => files.Values;
        }

        public async Task Add(StorageItem item)
        {
            if (item is AbstractFile file)
            {
                using var content = await file.GetContentAsync();
                var document = await vkClient.UploadDocumentAsync(file.Path.Replace(System.IO.Path.DirectorySeparatorChar, ':'), content);
                var remoteFile = new RemoteFile(document);
                files[remoteFile.Path] = remoteFile;

                Trace.WriteLine($"{document.Title} has been uploaded");
            }

            //LOG
        }

        public bool Contains(StorageItem item)
        {
            if (item is AbstractFile file)
            {
                if (files.TryGetValue(file.Path, out var targetFile))
                {
                    return file.Equals(targetFile);
                }
            }

            return false;
        }

        public async Task Remove(StorageItem item)
        {
            if (item is AbstractFile file)
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
        }

        private readonly Client vkClient;
        private readonly Dictionary<string, RemoteFile> files;
    }
}
