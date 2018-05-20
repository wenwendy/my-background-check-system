using System;
using MyBackgroundCheckService.Processor.DTOs;
using MyBackgroundCheckService.Processor.Senders;
using MyBackgroundCheckService.Processor.Transformers;
using Newtonsoft.Json;
using QueueService;

namespace MyBackgroundCheckService.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            const string InvitationQueueName = "invitation";

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
                        RemoveInvitationFromQueue(queueService);
                    }
                }
            }
        }

        private static void RemoveInvitationFromQueue(IQueueService queueService)
        {
            
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
    }
}
