using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.DAL;
using MyBackgroundCheckService.Library.DTOs;
using MyBackgroundCheckService.Library.Domain;
using Newtonsoft.Json;
using LanguageExt;
using static LanguageExt.Prelude;

namespace MyBackgroundCheckService.Api.Controllers
{
    [Route("api/[controller]")]
    public class InvitationController : Controller
    {
        private readonly IRepository _repository;
        private readonly IQueueService _queueService;
        private readonly AddInvitationCommandHandler _addInvitationCommandHandler;
        private readonly CommandGenerator _commandGenerator;

        public InvitationController(
            IRepository repository,
            IQueueService queueService
            )
        {
            _repository = repository;
            _queueService = queueService;
            // TODO: inject them
            _addInvitationCommandHandler = new AddInvitationCommandHandler(_repository);
            _commandGenerator = new CommandGenerator();
        }

        // responsibility:
        //   - provide a meaningful response basing on the request so that caller is clear on the next step
        //   - logging?
        [HttpPost]
        public IActionResult Add([FromBody] InvitationPostRequestBody invitation)
        {
            //TODO: replace with a logger and inject
            Console.WriteLine($"2_Service: Received an invitation request {JsonConvert.SerializeObject(invitation)}");

            return _commandGenerator.CreateAddInvitationCommand(invitation)
                     .Match(f => StatusCode(400, f.Message),
                            cmd => _addInvitationCommandHandler.Handle(cmd)
                            .Match(f =>
                            {
                                Console.WriteLine($"error is: {f.Message}");
                                // do not expose error details to API
                                return StatusCode(500, "Server error. Try again later.");
                            },
                                   u => StatusCode(200, "success")));
            
            /*
            // TODO MC: make this railway oriented... (request -> command)
            // Either<SomeError, AddInvitationCommand>
            // TODO MC: Also consider adding in memory tests around handler.*/

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
