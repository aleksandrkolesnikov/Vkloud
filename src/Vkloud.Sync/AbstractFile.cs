using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Vkloud.Sync
{
    public abstract class AbstractFile : IEquatable<AbstractFile>
    {
        public abstract string Path { get; init; }

        public abstract byte[] Hash { get; }

        public abstract Task<Stream> GetContentAsync();

        public virtual bool Equals(AbstractFile file)
        {
            if (Path == file.Path)
            {
                return Hash.SequenceEqual(file.Hash);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                AbstractFile item => Equals(item),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            // TODO: temporary return 0. I have the great idea for this method
            return 0;
        }
    }
}
