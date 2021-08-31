using System.Threading.Tasks;
using System.Linq;


namespace Vkloud.Sync
{
    public sealed class Synchronizer
    {
        public Synchronizer(IStorage left, IStorage right)
        {
            this.left = left;
            this.right = right;
        }

        public async Task Init()
        {
            var diff1 = right.Items.Except(left.Items);
            var diff2 = left.Items.Except(right.Items);

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
            left.Added += ItemAdded;
            left.Removed += ItemRemoved;

            right.Added += ItemAdded;
            right.Removed += ItemRemoved;
        }

        private void ItemRemoved(object sender, StorageEventArgs e)
        {
            var targetStorage = Receiver(e.Storage);
            if (targetStorage.Contains(e.Item))
            {
                targetStorage.Remove(e.Item);
            }
        }

        private void ItemAdded(object sender, StorageEventArgs e)
        {
            var targetStorage = Receiver(e.Storage);
            if (!targetStorage.Contains(e.Item))
            {
                targetStorage.Add(e.Item);
            }
        }

        private IStorage Receiver(IStorage storage)
        {
            return storage == left ? right : left;
        }

        private readonly IStorage left;
        private readonly IStorage right;
    }
}
