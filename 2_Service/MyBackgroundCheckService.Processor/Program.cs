using System;
using System.Threading.Tasks;
using MyBackgroundCheckService.Processor.Senders;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.DAL;


namespace MyBackgroundCheckService.Processor
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            // if using AWS, each task can be a lambda
            var t1 = ProcessInvitation();
            // can be a separate project to further mitigate node failure
            var t2 = UpdateStatusToDB();
            var t3 = QueueInvitationReceivedEvent();

            await Task.WhenAll(t1, t2, t3);
        }

        private static async Task ProcessInvitation()
        {
            Console.WriteLine("Invitation processor started. Awaiting ...");
            var invitationProcessor = new InvitationProcessor(new BobProviderSender(), new LocalFileQueueService());
            await invitationProcessor.Process();
        }

        private static async Task UpdateStatusToDB()
        {
            Console.WriteLine("Status DB updator started. Awaiting ...");
            var statusDBUpdator = new StatusDBUpdator(new LocalFileQueueService(), new Repository());
            await statusDBUpdator.Process();
        }

        private static async Task QueueInvitationReceivedEvent()
        {
            Console.WriteLine("Invitation received event poller started. Awaiting ...");
            var eventPoller = new EventPoller(new LocalFileQueueService(), new Repository());
            await eventPoller.Process();
        }

    }
}
