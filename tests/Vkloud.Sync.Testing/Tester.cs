using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Vkloud.Sync;


namespace SyncTests
{
    public class Tester
    {
        [Fact(DisplayName = "Test Initial Synchronization")]
        public async Task TestInitial()
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");

            foreach (var item in GetItems())
            {
                await storage1.Add(item);
            }

            var synchronizer = new Synchronizer(storage1, storage2);
            await synchronizer.Init();

            Assert.Equal(storage1.Items, storage2.Items);
        }

        [Fact(DisplayName = "Test Adding Synchronization")]
        public async Task TestAdding()
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");
            var synchronizer = new Synchronizer(storage1, storage2);

            await synchronizer.Init();

            foreach (var item in GetItems())
            {
                await storage1.Add(item);
            }

            Assert.Equal(storage1.Items, storage2.Items);
        }

        [Fact(DisplayName = "Test Removing Synchronization")]
        public async Task TestRemoving()
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");
            var synchronizer = new Synchronizer(storage1, storage2);

            await synchronizer.Init();

            foreach (var item in GetItems())
            {
                await storage1.Add(item);
            }

            var item1 = storage2.Items.First();
            await storage2.Remove(item1);

            Assert.Equal(storage1.Items, storage2.Items);
        }

        private static IEnumerable<SimpleItem> GetItems()
        {
            var random = new Random();

            for (var i = 0; i < 10; ++i)
            {
                var size = random.Next(10, 1024);
                var buffer = new byte[size];
                random.NextBytes(buffer);

                yield return new SimpleItem(buffer);
            }
        }
    }
}
