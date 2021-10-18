using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Vkloud.Sync;


namespace SyncTests
{
    public sealed class SynchronizerTester
    {
        [Theory(DisplayName = "Test Initial Synchronization")]
        [MemberData(nameof(GenerateFileSet))]
        public async Task TestInitialize(params AbstractFile[] files)
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");

            foreach (var file in files)
            {
                await storage1.Add(file);
            }

            var synchronizer = new Synchronizer(storage1, storage2);
            await synchronizer.Init();

            Assert.Equal(storage1.Files, storage2.Files);
        }

        [Theory(DisplayName = "Test Adding Synchronization")]
        [MemberData(nameof(GenerateFileSet))]
        public async Task TestAdding(params AbstractFile[] files)
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");
            var synchronizer = new Synchronizer(storage1, storage2);

            await synchronizer.Init();

            foreach (var item in files)
            {
                await storage1.Add(item);
            }

            Assert.Equal(storage1.Files, storage2.Files);
        }

        [Theory(DisplayName = "Test Removing Synchronization")]
        [MemberData(nameof(GenerateFileSet))]
        public async Task TestRemoving(params AbstractFile[] files)
        {
            var storage1 = new SimpleStorage("Storage 1");
            var storage2 = new SimpleStorage("Storage 2");
            var synchronizer = new Synchronizer(storage1, storage2);

            await synchronizer.Init();

            foreach (var item in files)
            {
                await storage1.Add(item);
            }

            foreach (var item in files)
            {
                await storage2.Remove(item);
            }

            Assert.Equal(storage1.Files, storage2.Files);
        }

        public static IEnumerable<object[]> GenerateFileSet()
        {
            yield return new object[] { CreateFile("File1") };

            yield return new object[] {
                                        CreateFile("File2.txt"),
                                        CreateFile("File3.log") };

            yield return new object[] {
                                        CreateFile("TestFile.log"),
                                        CreateFile("qwerty.cpp"),
                                        CreateFile("xyz.data") };
        }

        private static SimpleFile CreateFile(string path)
        {
            var rnd = new Random();
            var data = new byte[200];
            rnd.NextBytes(data);
            return new SimpleFile(data, path);
        }
    }
}
