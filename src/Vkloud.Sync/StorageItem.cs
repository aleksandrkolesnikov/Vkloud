using System;
using System.IO;
using System.Threading.Tasks;


namespace Vkloud.Sync
{
    public abstract class StorageItem : IEquatable<StorageItem>
    {
        public abstract Task<Stream> GetContentAsync();

        public virtual bool Equals(StorageItem item)
        {
            //TODO: Create and use sync version of GetContentAsync

            using var reader = new StreamReader(GetContentAsync().Result);
            var data = reader.ReadToEnd();

            using var reader2 = new StreamReader(item.GetContentAsync().Result);
            var data2 = reader2.ReadToEnd();

            return data == data2;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                StorageItem item => Equals(item),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            using var reader = new StreamReader(GetContentAsync().Result);
            var data = reader.ReadToEnd();

            return data.GetHashCode();
        }
    }
}
