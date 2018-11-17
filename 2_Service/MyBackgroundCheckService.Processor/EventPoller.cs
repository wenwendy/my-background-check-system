using System;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Library;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyBackgroundCheckService.Processor
{
    public class EventPoller
    {
        private readonly IRepository _repository;
        private readonly IQueueService _queueService;
        const string InvitationQueueName = "invitation";

        public EventPoller(IQueueService queueService, IRepository repository)
        {
            _queueService = queueService;
            _repository = repository;
        }

        public async Task Process()
        {
            while (true)
            {
                try
                {
                    var nextEventInDB = _repository.GetNextEvent();
                    _queueService.Named(InvitationQueueName).Add(JsonConvert.SerializeObject(nextEventInDB));
                    _repository.DeleteEvent(nextEventInDB.Id);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // do nothing
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
