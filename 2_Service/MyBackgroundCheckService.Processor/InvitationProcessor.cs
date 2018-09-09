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
        private readonly IRepository _repository;
        const string InvitationQueueName = "invitation";

        public InvitationProcessor(ISender sender, IQueueService queueService, IRepository repository)
        {
            _sender = sender;
            _queueService = queueService;
            _repository = repository;
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
                            break;

                        case SendResult.FailPermanently:
                            Console.WriteLine("Request failed permanently.");
                            // update status in postgresql
                            var invitaionEntity = new InvitationEntity
                            {
                                Id = invitation.Id,
                                ApplicantProfile = invitation.ApplicantProfile,
                                Status = "InvalidRequest"
                            };
                            _repository.UpSert(invitaionEntity);
                            _queueService.Named(InvitationQueueName).Remove(JsonConvert.SerializeObject(invitation));
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