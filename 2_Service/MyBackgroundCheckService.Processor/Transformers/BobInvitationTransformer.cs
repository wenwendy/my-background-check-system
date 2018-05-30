using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public class BobInvitationTransformer : IInvitationTransformer
    {
        public object Transform(InvitationDto invitation)
        {
            return new { Id = invitation.Id, CandidateProfile = invitation.ApplicantProfile };
        }

    }
}