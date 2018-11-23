using MyBackgroundCheckService.Library.DTOs;

namespace MyBackgroundCheckService.Library.Domain
{
    public class InvitationAggregate
    {
        public int Id { get; set; }

        public ApplicantProfile ApplicantProfile { get; set; }

        public string Status { get; set; }
    }


    //TODO: read json field from postgres and parse into DTO
    public class InvitationTemp
    {
        public int Id { get; set; }
        public string ApplicantProfile { get; set; }
        public string Status { get; set; }
    }
}
