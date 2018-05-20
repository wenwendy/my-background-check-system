using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public class BobTransformer : ITransformer
    {
        public object Transform(InvitationDto invitation)
        {
            return invitation;
        }
    }
}