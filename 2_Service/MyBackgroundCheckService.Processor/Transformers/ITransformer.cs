using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public interface ITransformer
    {
        object Transform(InvitationDto invitation);
    }
}