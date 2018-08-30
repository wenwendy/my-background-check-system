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
            _sender = new BobProviderSender();
        }
        
        public async Task Process()
        {
            var queueService = new LocalFileQueueService();//inject this
            
            //send till successful
            while (true)
            {
                var invitation = GetAnInvitationFromQueue(queueService, InvitationQueueName);//var invitation = queueService.NextItemInQueue(InvitationQueueName)

                if (invitation != null)
                {
                    var received = await _sender.Send(invitation);
                    
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
            //This needs to be 2, 4, 8, 16 etc.
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}