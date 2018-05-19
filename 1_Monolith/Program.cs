using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Monolith
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var invitation = new { id = 123, provider = "bob" };
            const string uri = "https://localhost:1111/api/addinvitation";
            var content = new StringContent(JsonConvert.SerializeObject(invitation), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = client.PostAsync(uri, content).Result; 
            var responseString = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(responseString);
         
        }
    }
}
