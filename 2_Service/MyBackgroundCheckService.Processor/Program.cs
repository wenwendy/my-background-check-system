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
            var t1 = ProcessInvitation();
            var t2 = UpdateStatusToDB();

            await Task.WhenAll(t1, t2);
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

    }
}
