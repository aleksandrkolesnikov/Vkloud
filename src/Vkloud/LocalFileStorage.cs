using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkloud.Sync;


namespace Vkloud
{
    sealed class LocalFileStorage : IFileStorage
    {
        public event EventHandler<FileStorageEventArgs> Added;
        public event EventHandler<FileStorageEventArgs> Removed;

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

            //files = new();
        }

        public IEnumerable<AbstractFile> Files => container.Files;

        public Task Add(AbstractFile file)
        {
            var subDirInfo = Directory.CreateDirectory(Path.Join(baseDirInfo.FullName, Path.GetDirectoryName(file.Path)));
            var fullFileName = Path.Join(subDirInfo.FullName, Path.GetFileName(file.Path));

            File.Create(fullFileName).Close();
            var barelyFile = new BarelyLocalFile(file, fullFileName);
            container.Add(barelyFile);
            //files[barelyFile.Path] = barelyFile;

            return Task.CompletedTask;
        }

        public bool Contains(AbstractFile file)
        {
            return container.Files.Contains(file);
        }

        public Task Remove(AbstractFile file)
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
                //TODO: add new file to container
                Added?.Invoke(this, new FileStorageEventArgs(this, new LocalFile(e.FullPath) { Path = e.Name }));
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            //TODO: deleting folder does not support

            var fake = container.CreateFake(e.FullPath.Replace(baseDirInfo.FullName + Path.DirectorySeparatorChar, ""));
            Removed?.Invoke(this, new FileStorageEventArgs(this, fake));
        }

        private static bool IsFile(string fullPath) => !File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);

        private readonly DirectoryInfo baseDirInfo;
        private readonly FileSystemWatcher watcher;
        private readonly LocalFileContainer container;
        //private readonly Dictionary<string, AbstractFile> files;
    }
}
