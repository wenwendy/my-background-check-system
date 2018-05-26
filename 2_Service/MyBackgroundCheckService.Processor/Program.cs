using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var invitationProcessor = new InvitationProcessor();
            await invitationProcessor.Process();
            
            //another thread
            var resultProcessor = new ResultProcessor();
            await resultProcessor.Process();

        }
    }
}
