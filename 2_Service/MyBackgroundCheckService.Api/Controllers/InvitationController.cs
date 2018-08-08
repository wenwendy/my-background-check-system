using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Library;
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
        public IActionResult Add([FromBody] Invitation invitation)
        {
            Console.WriteLine($"2_Service: Received an invitation request {JsonConvert.SerializeObject(invitation)}");

            // try x times before pushing to DLQ
            // what if DLQ pushing also failed?
            _queueService.AddToQueue("invitation", JsonConvert.SerializeObject(invitation));
            _repository.UpSert(invitation);
           
            return Ok($"Invitation: {JsonConvert.SerializeObject(invitation)} received");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine($"Check status for invitation {id}");

            var invitation = _repository.Get(id);

            return Ok(invitation);
        }

    }
}
