using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public interface IInvitationTransformer
    {
        object Transform(InvitationDto invitation);
    }
}