namespace MyBackgroundCheckService.Library.Domain
{
    public class InvitationAggregate
    {
        public InvitationAggregate(int id, ApplicantProfile applicantProfile, string status)
        {
            Id = id;
            ApplicantProfile = applicantProfile;
            Status = status;
        }


        public int Id { get; private set; }

        public ApplicantProfile ApplicantProfile { get; private set; }

        public string Status { get; private set; }
    }

}
