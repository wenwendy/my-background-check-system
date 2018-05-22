using System;
using System.Net.Http;

namespace Monolith.BackgroundCheck
{
    class Program
    {
        public static void Main(string[] args)
        {
            GetResults();
            Console.ReadKey();
            
            var invitation = GetInvitation();

            Console.WriteLine("posting an invitation ...");
            var receipt = SendInvitation(invitation);

            Console.WriteLine(receipt);
        }

        private static void GetResults()
        {
            const string uri = "http://localhost:26970/monolithapi/result";
            
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            var response = client.GetAsync("").Result;
            response.EnsureSuccessStatusCode();
            
            var content = response.Content.ReadAsStringAsync();
            
            Console.WriteLine(content.Result);
        }


        private static object GetInvitation()
        {
            return new
            {
                id = 123,
                provider = "bob"
            };
        }

        private static string SendInvitation(object invitation)
        {
            const string uri = "http://localhost:4777/api/invitation";

            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            var response = client.PostAsJsonAsync("", invitation).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }
    }
}
