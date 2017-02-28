using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DistributedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56737/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/batchdata/1");
                if (response.IsSuccessStatusCode)
                {
                    BatchData batch = await response.Content.ReadAsAsync<BatchData>(); // Install-Package Microsoft.AspNet.WebApi.Client
                    Console.WriteLine("{0}\t${1}\t{2}", batch.Key, batch.Data, batch.LastComplete);
                }
            }
        }
    }
}
