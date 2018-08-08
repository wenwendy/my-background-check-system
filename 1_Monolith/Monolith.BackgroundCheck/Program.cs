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
                        Console.WriteLine("Which invitation do you want to check (enter id)?");
                        var status = CheckStatus(int.Parse(Console.ReadLine()));
                        Console.WriteLine(status);
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
            Console.WriteLine("1. GET status of an existing invitation | 2. POST an invitation");
        }

        private static string CheckStatus(int id)
        {
            var uri = $"http://localhost:4777/api/invitation/{id}";
            
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            var response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            
            var content = response.Content.ReadAsStringAsync();
            
            return content.Result;
        }


        private static JObject GetInvitation()
        {
            try
            {
                using (var r = new StreamReader(@"../../../monolith-invitation.json"))
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
