using System.Collections.Generic;
using System.Linq;


namespace Vkloud.Model
{
    internal sealed class Node
    {
        public Node(Entry entry) => Data = entry;

        public Entry Data { get; set; }

        public IEnumerable<Entry> Children => nodes.Select(node => { return node.Data; });

        public Node TryAddChild(Entry entry)
        {
            foreach (var child in nodes)
            {
                if (child.Data.Name == entry.Name)
                {
                    return child;
                }
            }

            var newChild = new Node(entry);
            nodes.AddLast(newChild);

            return newChild;
        }

        public Node Find(string path)
        {
            foreach (var node in nodes)
            {
                if (node.Data.Name == path)
                {
                    return node;
                }
            }

            return null;
        }

        public bool Contains(string path)
        {
            foreach (var node in nodes)
            {
                if (node.Data.Name == path)
                {
                    return true;
                }
            }

            return false;
        }

        private readonly LinkedList<Node> nodes = new LinkedList<Node>();
    }
}
