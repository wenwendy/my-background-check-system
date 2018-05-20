using System;
using System.Net.Http;

namespace BobBackgroundCheckProvider.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            var bobResult = GetBackgroundCheckResult();
            
            const string uri = "http://localhost:4777/api/result";

            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            Console.WriteLine("sending background check result to requester...");
            var result = client.PostAsJsonAsync("", bobResult).Result;
        }

        private static object GetBackgroundCheckResult()
        {
            return new
            {
                Id = 123,
                Result = "Pass"
            };
        }
    }
}
