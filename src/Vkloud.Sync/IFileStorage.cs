using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Vkloud.Sync
{
    public interface IFileStorage
    {
        event EventHandler<FileStorageEventArgs> Added;
        event EventHandler<FileStorageEventArgs> Removed;

        IEnumerable<AbstractFile> Files { get; }

        Task Add(AbstractFile item);
        Task Remove(AbstractFile item);
        bool Contains(AbstractFile item);
    }
}
