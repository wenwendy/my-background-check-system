using System.Runtime.InteropServices.ComTypes;

namespace MyBackgroundCheckService.Processor.Senders
{
    public interface ISender
    {
        bool Send(object transformedInvitation);
    }
}