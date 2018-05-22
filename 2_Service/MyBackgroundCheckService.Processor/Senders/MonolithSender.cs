using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor.Senders
{
    public class MonolithSender : ISender
    {
        public async Task<bool> Send(object transformedInvitation)
        {
            const string uri = "http://localhost:26970/monolithapi/result";

            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            var response = await client.PostAsJsonAsync("", transformedInvitation);
            
            return (int)response.StatusCode >= 200;        
        }
    }
}