using System;


namespace Vkloud.Model
{
    internal struct Entry
    {
        public string Name { get; set; }
        public ulong? Size { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDirectory { get; set; }

    }
}
