using MyBackgroundCheckService.Library.DTOs;

namespace MyBackgroundCheckService.Library.DAL
{
    public class InvitationReveivedEventEntity
    {
        public int Id { get; set; }

        public string SerializedApplicantProfile { get; set; }

    }
}
