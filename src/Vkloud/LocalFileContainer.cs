using System.Linq;
using System.Collections.Generic;


namespace Vkloud
{
    record LocalFileWrapper(LocalFile LocalFile, byte[] Hash);

    sealed class LocalFileContainer
    {
        public LocalFileContainer(IEnumerable<LocalFile> items)
        {
            files = new(items.Select(f => new KeyValuePair<string, LocalFileWrapper>(f.Path, new(f, f.Hash))));
        }

        public IEnumerable<LocalFile> Files => files.Values.Select(f => f.LocalFile);

        public void Add(LocalFile file)
        {
            files[file.Path] = new LocalFileWrapper(file, file.Hash);
        }

        public void Remove(LocalFile file)
        {
            files.Remove(file.Path);
        }

        public FakeFile CreateFake(string path) => new(path, files[path].Hash);

        private readonly Dictionary<string, LocalFileWrapper> files;
    }
}
