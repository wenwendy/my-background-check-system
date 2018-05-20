using System;
using System.Threading;
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
        
        static void Main(string[] args)
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
                    var received = sender.Send(transformedInvitation);
                    if (received)
                    {
                        RemoveInvitationFromQueue(queueService, invitation);
                    }
                }

                RestABit();
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
        
        private static void RestABit()
        {
            Console.WriteLine("sleep for 10 seconds");
            
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
    }
}
