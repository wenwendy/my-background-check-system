using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BobBackgroundCheckProvider.Api.Controllers
{
    [Route("bobapi/[controller]")]
    public class InvitationController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody]BobInvitationDto invitation)
        {
            Console.WriteLine($"3_Provider: Received invitation: {JsonConvert.SerializeObject(invitation)}");

            switch (invitation.Id)                             
            {
                case 432:
                    return BadRequest("Invalid request");
                case 543:
                    return StatusCode(500, "Internal Server Error.");
                default:
                    return Ok();
            }
        }
    }

    public class BobInvitationDto
    {
        public int Id { get; set; }
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
