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
                // save to DB first because this won't result in down stream effects
                _repository.UpSert(new InvitationEntity { 
                    Id = invitation.Id, 
                    ApplicantProfile = invitation.ApplicantProfile, 
                    Status = "New" });
                // Q: if fails here for a long time (e.g. queue is down / has bugs), user may see misleading info based on GET
                // is not self healing when queue is back online
                _queueService.Named("invitation").Add(JsonConvert.SerializeObject(invitation));
                // machine can fail here
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // It's caller's responsibility to try again upon receiving 500 error.
                // can reache here when: 
                //   1. DB saving exceptioned - caller's retry can fix temporary error
                //   2. DB saving success but queue adding exceptioned. Upsert ensures DB saving is idempotent
                //   3. DB saving success and queue adding success but node failed before returning 200. Duplicate request can be added to queue which can be handled.
                return StatusCode(500, "Server error. Try again later.");
            }

            // 2xx response only returned when both DB and queue are updated successfully
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
        // potential callers: 2_Service InvitationProcessor / 3_Provider StatusUpdator
        // assumption: when processing request from 3_Provider, failure may not result in a retry.
        // Q: how to handle ^ ? choose whichever option more reliable?
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            Console.WriteLine($"Received a request to update status to {status} for invitation (id={id})");
            try
            {
                // option 1
                // pros:
                //   less point of failure - DB or node
                //   less delay when load is low
                // cons:
                //   not self healing when DB is down
                var invitation = _repository.Get(id);
                invitation.Status = status;
                _repository.UpSert(invitation);
                Console.WriteLine("Done");

                return Ok(invitation);

                // option 2
                // pros: 
                //   when request load is high (including batch operations), DB load is spreaded avoiding DB spike
                //   partial self healing. As long as queue is up, there's no need to rely on 3rd party re-sending status update
                // cons: 
                //   when request load is low, updated status takes slightly longer to reflect on GET.
                //   more point of failures. in case of queue / status DB updator failure, updated status takes longer to reflect.
                //   not fully self healing when queue is down.
                var statusUpdateRequest = new {
                    id = id,
                    status = status
                };
                _queueService
                    .Named("statusupdate")
                    .Add(JsonConvert.SerializeObject(statusUpdateRequest));

                return Accepted(statusUpdateRequest);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Server error, try again later");
            }
        }

    }
}
