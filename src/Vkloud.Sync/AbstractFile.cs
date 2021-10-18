using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace Vkloud.Sync
{
    public abstract class AbstractFile : IEquatable<AbstractFile>
    {
        public abstract string Path { get; init; }

        public virtual byte[] Hash
        {
            get
            {
                if (hash == null)
                {
                    using var md5 = MD5.Create();
                    using var content = GetContentAsync().Result;

                    hash = md5.ComputeHash(content);
                }

                return hash;
            }
        }

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

        private byte[] hash;
    }
}
