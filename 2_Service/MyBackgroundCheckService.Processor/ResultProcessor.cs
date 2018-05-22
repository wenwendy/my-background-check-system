using System;
using System.Threading.Tasks;
using MyBackgroundCheckService.Processor.DTOs;
using MyBackgroundCheckService.Processor.Senders;
using MyBackgroundCheckService.Processor.Transformers;
using Newtonsoft.Json;
using QueueService;

namespace MyBackgroundCheckService.Processor
{
    public class ResultProcessor
    {
        private readonly IResultTransformer _resultTransformer;
        private readonly ISender _sender;
        const string ResultQueueName = "result";

        public ResultProcessor()
        {
            _resultTransformer = new MonolithResultTransformer();
            _sender = new MonolithSender();
        }
        
        public async Task Process()
        {
            var queueService = new LocalFileQueueService();
            
            while (true)
            {
                var result = GetAResultFromQueue(queueService);

                if (result != null)
                {
                    var transformedResult = _resultTransformer.Transform(result);
                    var received = await _sender.Send(transformedResult);
                    
                    if (received)
                    {
                        RemoveResultFromQueue(queueService, result);
                    }
                }

                await RestABit();
            }
        }
        
        private ResultDto GetAResultFromQueue(IQueueService queueService)
        {
            ResultDto result = null;
            
            Console.WriteLine("getting item from queue ...");
            var resultJsonString = queueService.GetAQueueItem(ResultQueueName);

            if (string.IsNullOrEmpty(resultJsonString)) return result;
            
            result = JsonConvert.DeserializeObject<ResultDto>(resultJsonString);
            Console.WriteLine($"getting from queue: {JsonConvert.SerializeObject(result)}");

            return result;
        }
        
        private void RemoveResultFromQueue(IQueueService queueService, object result)
        {
            Console.WriteLine($"removing item {JsonConvert.SerializeObject(result)} ...");
            queueService.RemoveFromQueue(JsonConvert.SerializeObject(result), ResultQueueName);
            Console.WriteLine($"removed item {JsonConvert.SerializeObject(result)}");
        }
        
        private async Task RestABit()
        {
            Console.WriteLine("sleep for 10 seconds");
            
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}