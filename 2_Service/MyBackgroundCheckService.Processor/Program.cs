using System;
using System.Threading;
using System.Threading.Tasks;
using MyBackgroundCheckService.Processor.DTOs;
using MyBackgroundCheckService.Processor.Senders;
using MyBackgroundCheckService.Processor.Transformers;
using Newtonsoft.Json;
using QueueService;

namespace MyBackgroundCheckService.Processor
{
    class Program
    {
        const string InvitationQueueName = "invitation";
        
        static async Task Main(string[] args)
        {
            var queueService = new LocalFileQueueService();
            var transformer = new BobTransformer();
            var sender = new BobProviderSender();
            
            while (true)
            {
                var invitation = GetAnInvitationFromQueue(queueService, InvitationQueueName);

                if (invitation != null)
                {
                    var transformedInvitation = transformer.Transform(invitation);
                    var received = await sender.Send(transformedInvitation);
                    
                    if (received)
                    {
                        RemoveInvitationFromQueue(queueService, invitation);
                    }
                }

                await RestABit();
            }
        }

        private static InvitationDto GetAnInvitationFromQueue(IQueueService queueService, string InvitationQueueName)
        {
            InvitationDto invitation = null;
            
            Console.WriteLine("getting item from queue ...");
            var invitationJsonString = queueService.GetAQueueItem(InvitationQueueName);

            if (string.IsNullOrEmpty(invitationJsonString)) return invitation;
            
            invitation = JsonConvert.DeserializeObject<InvitationDto>(invitationJsonString);
            Console.WriteLine($"getting from queue: {invitation.Id}");

            return invitation;
        }
        
        private static void RemoveInvitationFromQueue(IQueueService queueService, InvitationDto invitation)
        {
            Console.WriteLine($"removing item {JsonConvert.SerializeObject(invitation)} ...");
            queueService.RemoveFromQueue(JsonConvert.SerializeObject(invitation), InvitationQueueName);
            Console.WriteLine($"removed item {JsonConvert.SerializeObject(invitation)}");
        }
        
        private static async Task RestABit()
        {
            Console.WriteLine("sleep for 10 seconds");
            
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
