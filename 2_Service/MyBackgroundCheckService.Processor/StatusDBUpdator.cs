using System;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Library;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyBackgroundCheckService.Processor
{
    public class StatusDBUpdator
    {
        private readonly IRepository _repository;
        private readonly IQueueService _queueService;
        const string StatusUpdateRequestQueueName = "statusupdate";

        public StatusDBUpdator(IQueueService queueService, IRepository repository)
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
                    var serializedRequest = GetAStatusUpdateRequestFromQueue();

                    if (serializedRequest != null)
                    {
                        var request = JsonConvert.DeserializeObject<dynamic>(serializedRequest);
                        int.TryParse(request["id"], out int id);

                        var invitation = _repository.Get(id);
                        invitation.Status = request["status"];

                        _repository.Update(invitation);

                        _queueService.Remove(serializedRequest);
                    }
                }
                catch (Exception e)
                {
                    // do not crash the process, wait for the next cycle and hope for self healing
                    Console.WriteLine(e.Message);
                }
                
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private string GetAStatusUpdateRequestFromQueue()
        {
            var request = _queueService.Named(StatusUpdateRequestQueueName).GetAQueueItem();

            if (string.IsNullOrEmpty(request)) return null;

            return request;
        }
    }
}
