using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Library.DTOs;
using MyBackgroundCheckService.Library.Domain;
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
        public IActionResult Add([FromBody] InvitationPostRequestBody invitation)
        {
            Console.WriteLine($"2_Service: Received an invitation request {JsonConvert.SerializeObject(invitation)}");

            // TODO MC: Get rid of exception handing in controller. (have exception dealt with in pipeline)
            try
            {
                // TODO MC: make this railway oriented... (request -> command)
                // TODO: use injection
                // Either<SomeError, AddInvitationCommand>
                // TODO MC: Also consider adding in memory tests around handler.
                AddInvitationCommand addInvitationCommand = new AddInvitationCommand(invitation);
                var commandHandler = new AddInvitationCommandHandler(_repository);
                commandHandler.Handle(addInvitationCommand);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // It's caller's responsibility to try again upon receiving 500 error.
                // can reache here when: 
                //   DB saving exceptioned - caller's retry can fix temporary error
                return StatusCode(500, "Server error. Try again later.");
            }

            return Ok();
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
                // pros: 
                //   less code changes when 3rd party communication interface changes
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
