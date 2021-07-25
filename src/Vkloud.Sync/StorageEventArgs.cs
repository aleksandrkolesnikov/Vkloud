using System;


namespace Vkloud.Sync
{
    public sealed class StorageEventArgs : EventArgs
    {
        public StorageEventArgs(IStorage storage, StorageItem item)
        {
            Storage = storage;
            Item = item;
        }

        public StorageItem Item { get; private set; }
        public IStorage Storage { get; private set; }
    }
}
