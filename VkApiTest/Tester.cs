using System;
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

        [Fact(DisplayName = "To Many Requests Per Seconds")]
        public void MakeManyRequests()
        {
            void TestAction()
            {
                var client = VkClient.Get;
                var tasks = new List<Task<List<Document>>>();

                for (int i = 0; i < 80; ++i)
                {
                    var docs = client.GetDocuments();
                    tasks.Add(docs);
                }

                Task.WaitAll(tasks.ToArray());
            }

            var ex = Assert.Throws<AggregateException>(TestAction);
            Assert.Equal(typeof(TooManyRequestsPerSecond), ex.InnerException.GetType());
            Assert.Equal("Too many requests per second", ex.InnerException.Message);
        }
    }
}
