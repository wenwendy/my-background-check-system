namespace MyBackgroundCheckService.Library.DTOs
{
    // // TODO MC: rename to something like: InvitationPostBody
    // (and rename all other DTO's to what they actually are.)
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