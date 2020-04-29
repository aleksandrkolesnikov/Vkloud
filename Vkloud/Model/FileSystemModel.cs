using System;
using System.Collections.Generic;
using VkApi;


namespace Vkloud.Model
{
    internal sealed class FileSystemModel
    {
        public FileSystemModel(Client client)
        {
            this.client = client;
            CurrentPath = "/";
            rootNode = new Node(new Entry{ Name = "/", Size = null, DateTime = DateTime.UtcNow, IsDirectory = true });

            Init();
        }

        public string CurrentPath { get; set; }

        public IEnumerable<Entry> Entries
        {
            get
            {
                var items = new List<string>(CurrentPath.Split("/"));
                var node = Walk(rootNode, items);

                return node.Children;
            }
        }

        private async void Init()
        {
            var docs = await client.GetDocuments();
            foreach (var doc in docs)
            {
                AddPath(doc.Title);
            }
        }

        private void AddPath(string path)
        {
            var items = new List<string>(path.Split("/"));
            AddPath(rootNode, items);
        }

        private void AddPath(Node root, List<string> items)
        {
            var item = items[0];
            items.Remove(item);
            var child = root.TryAddChild(new Entry { Name = item, Size = null });

            if (items.Count != 0)
            {
                AddPath(child, items);
            }
        }

        private Node Walk(Node root, List<string> items)
        {
            var item = items[0];
            items.Remove(item);
            var node = root.Find(item);

            if (items.Count == 0)
            {
                return node;
            }

            if (node != null)
            {
                return Walk(node, items);
            }

            return null;
        }

        private readonly Client client;
        private readonly Node rootNode;
    }
}
