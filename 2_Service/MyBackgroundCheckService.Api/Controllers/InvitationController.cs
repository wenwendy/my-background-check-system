using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Library.DTOs;
using Newtonsoft.Json;

namespace MyBackgroundCheckService.Api.Controllers
{
    [Route("api/[controller]")]
    public class InvitationController : Controller
    {
        private readonly IRepository _repository;
        private readonly IQueueService _queueService;

        public InvitationController(IRepository repository, IQueueService queueService)
        {
            _repository = repository;
            _queueService = queueService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] InvitationDto invitation)
        {
            Console.WriteLine($"2_Service: Received an invitation request {JsonConvert.SerializeObject(invitation)}");

            try
            {
                _queueService.Named("invitation").Add(JsonConvert.SerializeObject(invitation));
                _repository.UpSert(new InvitationEntity { Id = invitation.Id, ApplicantProfile = invitation.ApplicantProfile, Status = "New" });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // rollback to previous state??
                // OR save to queue only. have another processor to write to DB and send to 3_Provider
                return StatusCode(500, "Server error. Try again later.");
            }

            return Accepted();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine($"Check status for invitation {id}");

            var invitation = _repository.Get(id);

            return Ok(invitation);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            Console.WriteLine($"Received a request to update status to {status} for invitation (id={id})");
            try
            {
                var invitation = _repository.Get(id);
                invitation.Status = status;
                _repository.UpSert(invitation);
                Console.WriteLine("Done");

                return Ok(invitation);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server error, try again later");
            }
        }

    }
}
