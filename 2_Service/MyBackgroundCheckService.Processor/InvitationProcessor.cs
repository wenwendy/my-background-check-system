using System.Threading.Tasks;
using System;
using MyBackgroundCheckService.Processor.DTOs;
using MyBackgroundCheckService.Processor.Senders;
using MyBackgroundCheckService.Processor.Transformers;
using Newtonsoft.Json;
using QueueService;

namespace MyBackgroundCheckService.Processor
{
    public class InvitationProcessor
    {
        private readonly IInvitationTransformer _invitationTransformer;
        private readonly ISender _sender;
        const string InvitationQueueName = "invitation";

        public InvitationProcessor()
        {
            _invitationTransformer = new BobInvitationTransformer();
            _sender = new BobProviderSender();
        }
        
        public async Task Process()
        {
            var queueService = new LocalFileQueueService();
            
            while (true)
            {
                var invitation = GetAnInvitationFromQueue(queueService, InvitationQueueName);

                if (invitation != null)
                {
                    var transformedInvitation = _invitationTransformer.Transform(invitation);
                    var received = await _sender.Send(transformedInvitation);
                    
                    if (received)
                    {
                        RemoveInvitationFromQueue(queueService, invitation);
                    }
                }

                await RestABit();
            }
        }
        
        private InvitationDto GetAnInvitationFromQueue(IQueueService queueService, string InvitationQueueName)
        {
            InvitationDto invitation = null;
            
            //Console.WriteLine("2_Service: Getting item from invitation queue ...");
            var invitationJsonString = queueService.GetAQueueItem(InvitationQueueName);

            if (string.IsNullOrEmpty(invitationJsonString)) return invitation;
            
            invitation = JsonConvert.DeserializeObject<InvitationDto>(invitationJsonString);
            Console.WriteLine($"2_Service: Found an invitation to process: {JsonConvert.SerializeObject(invitation)}");

            return invitation;
        }
        
        private void RemoveInvitationFromQueue(IQueueService queueService, object invitation)
        {
            //Console.WriteLine($"2_Service: removing item {JsonConvert.SerializeObject(invitation)} ...");
            queueService.RemoveFromQueue(JsonConvert.SerializeObject(invitation), InvitationQueueName);
            Console.WriteLine($"2_Service: Removed processed invitation {JsonConvert.SerializeObject(invitation)}");
        }
        
        private async Task RestABit()
        {
            //Console.WriteLine("sleep for 10 seconds");
            
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}