using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor.Senders
{
    public interface ISender// or IConnector
    {
        Task<SendResult> Send(object transformedInvitation);
    }

    public enum SendResult
    {
        Success,
        TryAgain,
        FailPermanently
    }
}