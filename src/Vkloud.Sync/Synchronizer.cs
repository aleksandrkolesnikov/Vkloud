using System.Threading.Tasks;
using System.Linq;


namespace Vkloud.Sync
{
    public sealed class Synchronizer
    {
        public Synchronizer(IFileStorage left, IFileStorage right)
        {
            this.left = left;
            this.right = right;
        }

        public async Task Init()
        {
            var diff1 = right.Files.Except(left.Files);
            var diff2 = left.Files.Except(right.Files);

            foreach (var item in diff1)
            {
                await left.Add(item);
            }

            foreach (var item in diff2)
            {
                await right.Add(item);
            }

            ConnectStorages();
        }

        private void ConnectStorages()
        {
            left.Added += AddedFileHandler;
            left.Removed += RemovedFileHandler;

            right.Added += AddedFileHandler;
            right.Removed += RemovedFileHandler;
        }

        private void RemovedFileHandler(object sender, FileStorageEventArgs e)
        {
            var targetStorage = Receiver(e.Storage);
            if (targetStorage.Contains(e.File))
            {
                targetStorage.Remove(e.File);
            }
        }

        private void AddedFileHandler(object sender, FileStorageEventArgs e)
        {
            var targetStorage = Receiver(e.Storage);
            if (!targetStorage.Contains(e.File))
            {
                targetStorage.Add(e.File);
            }
        }

        private IFileStorage Receiver(IFileStorage storage)
        {
            return storage == left ? right : left;
        }

        private readonly IFileStorage left;
        private readonly IFileStorage right;
    }
}
