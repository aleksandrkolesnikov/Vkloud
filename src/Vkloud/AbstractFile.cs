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
                    var hash = file.Hash;

                    return hash.SequenceEqual(Hash);
                }

                return false;
            }

            return base.Equals(item);
        }

        public override int GetHashCode()
        {
            return 0;
            //return Convert.ToHexString(Hash).GetHashCode();
        }

        public virtual byte[] Hash
        {
            get
            {
                using var md5 = MD5.Create();
                using var content = GetContentAsync().Result;

                return md5.ComputeHash(content);
            }
        }
    }
}
