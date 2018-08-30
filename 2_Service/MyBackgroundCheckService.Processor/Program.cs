using System;
using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor
{
    static class Program
    {
        static async Task Main(string[] args)
        {   
            var t1 = ProcessInvitation();
            var t2 = ProcessResult();

            await Task.WhenAll(t1, t2);
        }

        private static async Task ProcessInvitation()
        {
            Console.WriteLine("Invitation processor started. Awaiting ...");
            var invitationProcessor = new InvitationProcessor();
            await invitationProcessor.Process();
        }
        
        private static async Task ProcessResult()
        {
            Console.WriteLine("Result processor started. Awaiting ...");
            var resultProcessor = new ResultProcessor();
            await resultProcessor.Process();
        }
    }
}
