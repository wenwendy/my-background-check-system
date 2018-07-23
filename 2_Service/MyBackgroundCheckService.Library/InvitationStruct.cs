using System;
using System.Collections.Generic;
using System.Text;

namespace MyBackgroundCheckService.Library
{
    public class Invitation
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
}
