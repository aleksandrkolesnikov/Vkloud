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
            files = new(documents.Select(document => new RemoteFile(document)));

            Trace.WriteLine($"RemoteStorage has {files.Count} files");
        }

        public IEnumerable<StorageItem> Items
        {
            get => files;
        }

        public async Task Add(StorageItem item)
        {
            if (item is AbstractFile file)
            {
                using var content = await file.GetContentAsync();
                var document = await vkClient.UploadDocumentAsync(file.Path.Replace(System.IO.Path.DirectorySeparatorChar, ':'), content);
                files.Add(new RemoteFile(document));
                //files.AddLast(new RemoteFile(document));

                Trace.WriteLine($"{document.Title} has been uploaded");
            }

            //LOG
        }

        public bool Contains(StorageItem item)
        {
            return files.Contains(item);
        }

        public async Task Remove(StorageItem item)
        {
            var f = item as AbstractFile;

            var t = files.First(file => file.Path == f.Path && file.Hash.SequenceEqual(f.Hash));
            await vkClient.RemoveDocumentAsync(t.Document);
            files.RemoveWhere(file => file.Path == f.Path && file.Hash.SequenceEqual(f.Hash));

            Trace.WriteLine($"{t.Document.Title} has been removed");
        }

        private readonly Client vkClient;
        private readonly HashSet<RemoteFile> files;
    }
}
