using System;
using System.Security.Cryptography;
using System.Linq;
using Vkloud.Sync;


namespace Vkloud
{
    abstract class AbstractFile : StorageItem
    {
        public abstract string Path { get; init; }

        public override bool Equals(StorageItem item)
        {
            if (item is AbstractFile file)
            {
                if (Path == file.Path)
                {
                    return Hash.SequenceEqual(file.Hash);
                }

                return false;
            }

            return base.Equals(item);
        }

        public override int GetHashCode()
        {
            //return 0;
            return Convert.ToHexString(Hash).GetHashCode();
        }

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

        private byte[] hash;
    }
}
