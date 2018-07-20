using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Monolith.BackgroundCheck
{
    class Program
    {
        public static void Main(string[] args)
        {
            PrintHelp();
            
            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        GetResults();
                        PrintHelp();
                        break;

                    case "2":
                        var invitation = GetInvitation();

                        Console.WriteLine("1_Monolith: Sending an invitation to 2_Service ...");
                        var receipt = SendInvitation(invitation);

                        Console.WriteLine($"1_Monolith: Received response from 2_Service: {receipt}");
                        PrintHelp();
                        break;
                }
            } while (true);

        }

        private static void PrintHelp()
        {
            Console.WriteLine("1. Print all results | 2. Invite");
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


        private static JObject GetInvitation()
        {
            try
            {
                using (var r = new StreamReader(@"monolith-invitation.json"))
                {
                    var invitation = r.ReadToEnd();
                    
                    return JObject.Parse(invitation);
                }   
            }
            catch(FileNotFoundException)
            {
                return new JObject();
            }
        }

        private static string SendInvitation(JObject invitation)
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
