using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using VkApi;


namespace VkApiTest
{
    public class Tester
    {
        [Fact(DisplayName = "Get All Documents")]
        public void GetAllDocuments()
        {
            var client = VkClient.Get;
            var docs = client.GetDocuments().Result;

            Assert.NotEmpty(docs);
        }

        [Fact(DisplayName = "Upload One File")]
        public void UploadOneFile()
        {
            var client = VkClient.Get;
            var doc = client.AddDocument("test_data/Beginning Rust.pdf").Result;

            Assert.Equal("Beginning Rust.pdf", doc.Title);
        }

        [Fact(DisplayName = "Upload Many Files")]
        public void UploadManyFiles()
        {
            var client = VkClient.Get;
            var files = new SortedSet<string>
                {
                    "algoritmy-spravochnik-s-primerami-2e.pdf",
                    "Beginning Rust.pdf",
                    "mongodb-v-deystvii.pdf",
                    "Olifer_V_G__Olifer_N_A_-_Kompyuternye_seti_P.pdf",
                    "sql полное руководство.pdf",
                    "¬недрение зависимостей в .NET   2014.pdf"
                };
            var docs = new ConcurrentBag<string>();

            async Task<Document> Selector(string file)
            {
                var doc = await client.AddDocument($"test_data/{file}");
                docs.Add(doc.Title);

                return doc;
            }

            var tasks = files.Select(Selector);
            Task.WaitAll(tasks.ToArray());
            var actual = new SortedSet<string>(docs);

            Assert.Equal(files, actual);
        }

        [Fact(DisplayName = "Delete Document")]
        public void DeleteDocument()
        {

        }

        [Fact(DisplayName = "Many Requests to API")]
        public void MakeManyRequests()
        {
            var client = VkClient.Get;

            try
            {
                var docs = client.GetDocuments().Result;
                var docs1 = client.GetDocuments().Result;
                var docs2 = client.GetDocuments().Result;
                var docs3 = client.GetDocuments().Result;
                var docs5 = client.GetDocuments().Result;
                var docs6 = client.GetDocuments().Result;
                var docs7 = client.GetDocuments().Result;
                var docs8 = client.GetDocuments().Result;
                var docs9 = client.GetDocuments().Result;
                var docs10 = client.GetDocuments().Result;
                var docs11 = client.GetDocuments().Result;
                var docs12 = client.GetDocuments().Result;
            }
            catch (VkApi.Exceptions.VkException ex)
            {
            }
        }
    }
}
