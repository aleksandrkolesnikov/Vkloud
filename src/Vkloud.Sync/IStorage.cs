using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Vkloud.Sync
{
    public interface IStorage
    {
        event EventHandler<StorageEventArgs> Added;
        //event EventHandler<string> Changed;
        event EventHandler<StorageEventArgs> Removed;

        IEnumerable<StorageItem> Items { get; }

        Task Add(StorageItem item);
        Task Remove(StorageItem item);
        bool Contains(StorageItem item);
    }
}
