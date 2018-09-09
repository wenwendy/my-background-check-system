using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor.Senders
{
    public class BobProviderSender : ISender
    {
        public async Task<SendResult> Send(object invitation)
        {
            const string uri = "http://localhost:58527/bobapi/invitation";

            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            var response = await client.PostAsJsonAsync("", invitation);

            Console.WriteLine($"2_Service: Response from Provider is {response.StatusCode}");
            
            return Convert.ToInt32(response.StatusCode) >= 500 ? SendResult.TryAgain 
                              : Convert.ToInt32(response.StatusCode) >= 400 ? SendResult.FailPermanently
                              : SendResult.Success;
        }
    }
}