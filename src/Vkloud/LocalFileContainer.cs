using System.Linq;
using System.Collections.Generic;


namespace Vkloud
{
    sealed class LocalFileContainer
    {
        public LocalFileContainer(IEnumerable<LocalFile> items)
        {
            files = new(items);
            hashes = new(files.Select(file => new KeyValuePair<string, byte[]>(file.Path, file.Hash)));
        }

        public IEnumerable<LocalFile> Files => files;

        public void Add(LocalFile file)
        {
            files.Add(file);
            hashes.Add(file.Path, file.Hash);
        }

        public void Remove(LocalFile file)
        {
            files.Remove(file);
            hashes.Remove(file.Path);
        }

        public FakeFile CreateFake(string path) => new(path, hashes[path]);
        
        private readonly HashSet<LocalFile> files;
        private readonly Dictionary<string, byte[]> hashes;
    }
}
