using System.Linq;
using System.Collections.Generic;
using Vkloud.Sync;


namespace Vkloud
{
    record LocalFileWrapper(AbstractFile File, byte[] Hash);

    sealed class LocalFileContainer
    {
        public LocalFileContainer(IEnumerable<AbstractFile> files_)
        {
            files = new(files_.Select(f => new KeyValuePair<string, LocalFileWrapper>(f.Path, new(f, f.Hash))));
        }

        public IEnumerable<AbstractFile> Files => files.Values.Select(f => f.File);

        public void Add(AbstractFile file)
        {
            files[file.Path] = new LocalFileWrapper(file, file.Hash);
        }

        public void Remove(AbstractFile file)
        {
            files.Remove(file.Path);
        }

        public FakeFile CreateFake(string path) => new(path, files[path].Hash);

        private readonly Dictionary<string, LocalFileWrapper> files;
    }
}
