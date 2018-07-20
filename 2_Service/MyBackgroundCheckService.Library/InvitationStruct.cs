using System;
using System.Collections.Generic;
using System.Text;

namespace MyBackgroundCheckService.Library
{
    public struct Invitation
    {
        public int Id { get; set; }
        
        public string ApplicantProfile { get; set; }

        public string Status { get; set; }
    }
}
