using System.Threading.Tasks;
using System;
using MyBackgroundCheckService.Library.DTOs;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Processor.Senders;
using Newtonsoft.Json;
using MyBackgroundCheckService.Library;

namespace MyBackgroundCheckService.Processor
{
    public class InvitationProcessor
    {
        private readonly ISender _sender;
        private readonly IQueueService _queueService;
        const string InvitationQueueName = "invitation";

        public InvitationProcessor(ISender sender, IQueueService queueService)
        {
            _sender = sender;
            _queueService = queueService;
        }
        
        public async Task Process()
        {
            while (true)
            {
                var invitation = GetAnInvitationFromQueue();

                if (invitation != null)
                {
                    var result = await _sender.Send(invitation);
                    
                    switch (result)
                    {
                        case SendResult.Success:
                            _queueService.Named(InvitationQueueName).Remove(JsonConvert.SerializeObject(invitation));
                            // code can fail to remove from queue
                            // Invitation in queue will be picked up again > sent to Provider > Response Success again
                            break;

                        case SendResult.FailPermanently:
                            Console.WriteLine("Request failed permanently.");
                            // update status via API
                            // Do not update DB directly here. Do not tie up DB with Processor
                            var putEndPoint = $"http://localhost:4777/api/invitation/{invitation.Id}/status";
                            var result = await new HttpClient().PutAsJsonAsync(putEndPoint, "InvalidRequest");
                
                            // call status update API, upon 500, retry till 2xx received
                            if (response.StatusCode == StatusCode.OK)
                            {
                                _queueService.Named(InvitationQueueName).Remove(JsonConvert.SerializeObject(invitation));
                                // code can fail to remove from queue
                                // Invitation in queue will be picked up again > sent to Provider > Response FailPermanently again
                            }
                            else
                            {
                                // code can reach here if:
                                // 1. PUT API is down. 
                                // 2. DB update failed
                                // Do nothing. 
                                // Invitation in queue will be picked up again > sent to Provider > Response FailPermanently again
                            }
                            break;

                        case SendResult.TryAgain:
                            // do nothing for x times, before moving to DLQ
                            break;
                    }
                }

                await RestABit();
            }
        }
        
        private InvitationDto GetAnInvitationFromQueue()
        {
            InvitationDto invitation = null;

            var invitationJsonString = _queueService.Named(InvitationQueueName).GetAQueueItem();

            if (string.IsNullOrEmpty(invitationJsonString)) return invitation;

            invitation = JsonConvert.DeserializeObject<InvitationDto>(invitationJsonString);
            
            Console.WriteLine($"2_Service: Found an invitation to process: {JsonConvert.SerializeObject(invitation)}");

            return invitation;
        }

        private async Task RestABit()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}