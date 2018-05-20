using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace MyBackgroundCheckService.Processor.Senders
{
    public interface ISender
    {
        Task<bool> Send(object transformedInvitation);
    }
}