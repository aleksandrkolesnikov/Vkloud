using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace SyncTests
{
    sealed class SimpleStorage : IStorage
    {
        public event EventHandler<StorageEventArgs> Added;
        //public event EventHandler<string> Changed;
        public event EventHandler<StorageEventArgs> Removed;

        public SimpleStorage(string name)
        {
            this.name = name;

            /*items.Add(new SimpleItem(Guid.NewGuid()));
            items.Add(new SimpleItem(Guid.NewGuid()));
            items.Add(new SimpleItem(Guid.NewGuid()));
            items.Add(new SimpleItem(Guid.NewGuid()));
            items.Add(new SimpleItem(Guid.NewGuid()));
            items.Add(new SimpleItem(Guid.NewGuid()));*/
        }

        public IEnumerable<StorageItem> Items => items;

        public async Task Add(StorageItem item)
        {
            var content = await item.GetContentAsync();
            var st = new MemoryStream();
            await content.CopyToAsync(st);
            var simpleItem = new SimpleItem(st.ToArray());
            items.Add(simpleItem);

            Added?.Invoke(this, new StorageEventArgs(this, simpleItem));
        }

        public Task Remove(StorageItem item)
        {
            var t = items.Find(i =>
                        {
                            if (item.Equals(i)) return true;
                            return false;
                        });

            items.Remove(t);

            Removed?.Invoke(this, new StorageEventArgs(this, t));

            return Task.CompletedTask;
        }

        public bool Contains(StorageItem item)
        {
            return items.Contains(item as SimpleItem);
        }

        private readonly string name;
        private readonly List<SimpleItem> items = new();
    }
}
