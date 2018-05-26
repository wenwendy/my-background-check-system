using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BobBackgroundCheckProvider.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("3_Provider: Send background check result back?");
                Console.ReadKey();

                var bobResult = GetBackgroundCheckResult();

                const string uri = "http://localhost:4777/api/result";

                var client = new HttpClient
                {
                    BaseAddress = new Uri(uri)
                };

                Console.WriteLine("3_Provider: Sending background check result to requester...");
                var result = client.PostAsJsonAsync("", bobResult).Result;
                Console.WriteLine("Done!");
            }
        }

        private static JObject GetBackgroundCheckResult()
        {
            try
            {
                using (var r = new StreamReader(@"provider-result.json"))
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
    }
}
