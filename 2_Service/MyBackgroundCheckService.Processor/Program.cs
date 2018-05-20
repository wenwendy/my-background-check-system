using System;
using QueueService;

namespace MyBackgroundCheckService.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("getting item from queue ...");
            
            const string InvitationQueueName = "invitation";
            
            var queue = new LocalFileQueueService();
            Console.WriteLine($"getting from queue: {queue.GetAQueueItem(InvitationQueueName)}");
        }
    }
}
