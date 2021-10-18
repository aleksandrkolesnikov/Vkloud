using System;


namespace Vkloud.Sync
{
    public sealed class FileStorageEventArgs : EventArgs
    {
        public FileStorageEventArgs(IFileStorage storage, AbstractFile file)
        {
            Storage = storage;
            File = file;
        }

        public AbstractFile File { get; private set; }
        public IFileStorage Storage { get; private set; }
    }
}
