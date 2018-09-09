namespace MyBackgroundCheckService.Library.DTOs
{
    public class Invitation//Entity
    {
        public int Id { get; set; }

        public ApplicantProfile ApplicantProfile { get; set; }

        public string Status { get; set; }
    }

    public class ApplicantProfile
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
    }

    //TODO: read json field from postgres and parse into DTO
    public class InvitationTemp
    {
        public int Id { get; set; }
        public string ApplicantProfile { get; set; }
        public string Status { get; set; }
    }
}
