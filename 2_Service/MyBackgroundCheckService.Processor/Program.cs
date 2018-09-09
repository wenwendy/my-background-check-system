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

            await Task.WhenAll(t1);
        }

        private static async Task ProcessInvitation()
        {
            Console.WriteLine("Invitation processor started. Awaiting ...");
            var invitationProcessor = new InvitationProcessor(new BobProviderSender(), new LocalFileQueueService(), new Repository());
            await invitationProcessor.Process();
        }

    }
}
