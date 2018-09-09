namespace MyBackgroundCheckService.Library.DTOs
{
    public class InvitationDto
    {
        public int Id {get; set;}
        public ApplicantProfile ApplicantProfile { get; set; }
    }

    public class ApplicantProfile
    {
        public string Name { get; set; }
        public string Dob { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
    }
}