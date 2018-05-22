using System;
using System.Net.Http;

namespace Monolith.BackgroundCheck
{
    class Program
    {
        public static void Main(string[] args)
        {
            var invitation = GetInvitation();

            Console.WriteLine("posting an invitation ...");
            var receipt = SendInvitation(invitation);

            Console.WriteLine(receipt);
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
