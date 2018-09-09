using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BobBackgroundCheckProvider.StatusUpdator
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

                var putEndPoint = $"http://localhost:4777/api/invitation/{bobResult["id"]}/status";

                Console.WriteLine($"3_Provider: Sending background check result to {putEndPoint} ...");
                var result = new HttpClient().PutAsJsonAsync(putEndPoint, bobResult["status"]).Result;
                Console.WriteLine($"Done! Result: {result}");
            }
        }

        private static JObject GetBackgroundCheckResult()
        {
            try
            {
                using (var r = new StreamReader(@"invitation-status.json"))
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
