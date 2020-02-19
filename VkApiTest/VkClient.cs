using VkApi;
using System.Xml;


namespace VkApiTest
{
    internal static class VkClient
    {
        static VkClient()
        {
            var doc = new XmlDocument();
            doc.Load("Settings.xml");
            var node = doc.DocumentElement.SelectSingleNode("/Settings/Credentials");
            var password = node.Attributes["password"].Value;
            var username = node.Attributes["username"].Value;

            Get = new Client(username, password);
        }

        public static Client Get { get; }
    }
}
