using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace SyncTests
{
    sealed class SimpleStorage : IFileStorage
    {
        public event EventHandler<FileStorageEventArgs> Added;
        public event EventHandler<FileStorageEventArgs> Removed;

        public SimpleStorage(string name)
        {
            this.name = name;
        }

        public IEnumerable<AbstractFile> Files => items.Values;

        public async Task Add(AbstractFile file)
        {
            using var content = await file.GetContentAsync();
            var st = new MemoryStream();
            await content.CopyToAsync(st);
            var simpleItem = new SimpleFile(st.ToArray(), file.Path);
            items.Add(simpleItem.Path, simpleItem);

            Added?.Invoke(this, new FileStorageEventArgs(this, simpleItem));
        }

        public Task Remove(AbstractFile file)
        {
            if (items.TryGetValue(file.Path, out var targetFile))
            {
                items.Remove(targetFile.Path);
                Removed?.Invoke(this, new FileStorageEventArgs(this, targetFile));

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public bool Contains(AbstractFile file)
        {
            if (items.TryGetValue(file.Path, out var targetFile))
            {
                return file.Equals(targetFile);
            }

            return false;
        }

        private readonly string name;
        private readonly Dictionary<string, AbstractFile> items = new();
    }
}
