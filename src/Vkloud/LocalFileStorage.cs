using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;
using System.Diagnostics;


namespace Vkloud
{
    sealed class LocalFileStorage : IStorage
    {
        public event EventHandler<StorageEventArgs> Added;
        public event EventHandler<string> Changed;
        public event EventHandler<StorageEventArgs> Removed;

        public LocalFileStorage(string path)
        {
            baseDirInfo = Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);

            watcher = new(path);
            watcher.IncludeSubdirectories = true;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.EnableRaisingEvents = true;

            container = new(baseDirInfo
                                .EnumerateFiles("*", SearchOption.AllDirectories)
                                .Select(fileInfo => new LocalFile(fileInfo.FullName)
                                {
                                    Path = fileInfo.FullName.Replace(baseDirInfo.FullName + Path.DirectorySeparatorChar, "")
                                }));
        }

        public IEnumerable<StorageItem> Items => container.Files;

        public async Task Add(StorageItem item)
        {
            if (item is AbstractFile file)
            {
                var subDirInfo = Directory.CreateDirectory(Path.Join(baseDirInfo.FullName, Path.GetDirectoryName(file.Path)));
                var fullFileName = Path.Join(subDirInfo.FullName, Path.GetFileName(file.Path));

                using (var fileStream = File.OpenWrite(fullFileName))
                using (var content = await file.GetContentAsync())
                {
                    await content.CopyToAsync(fileStream);
                }

                var localFile = new LocalFile(fullFileName) { Path = fullFileName.Replace(baseDirInfo.FullName + Path.DirectorySeparatorChar, "") };
                container.Add(localFile);

                Trace.WriteLine($"{file.Path} has been added");
            }
        }

        public bool Contains(StorageItem item)
        {
            return container.Files.Contains(item);
        }

        public Task Remove(StorageItem item)
        {
            //File.Delete(Path.Join(baseDirInfo.FullName, item.Path));


            //TODO: Should not use Task.CompletedTask, because File.Delete may throw exceptions,
            // but Task.CompletedTask means successfully finished task
            return Task.CompletedTask;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (IsFile(e.FullPath))
            {
                Added?.Invoke(this, new StorageEventArgs(this, new LocalFile(e.FullPath) { Path = e.Name }));
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var fake = container.CreateFake(e.FullPath.Replace(baseDirInfo.FullName + Path.DirectorySeparatorChar, ""));
            Removed?.Invoke(this, new StorageEventArgs(this, fake));
        }

        private static bool IsFile(string fullPath) => !File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);

        private readonly DirectoryInfo baseDirInfo;
        private readonly FileSystemWatcher watcher;
        private readonly LocalFileContainer container;
    }
}
