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
        Success,//initial request successful OR following up request returned "item aready existed" 
        TryAgain,//temp errors on Provider side
        FailPermanently//requires request being modified. e.g. vaidation errors.
    }
}