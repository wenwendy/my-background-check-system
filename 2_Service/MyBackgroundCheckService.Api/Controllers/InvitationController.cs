using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Api.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QueueService;

namespace MyBackgroundCheckService.Api.Controllers
{
    
    [Route("api/[controller]")]
    public class InvitationController : Controller
    {
        private const string InvitationQueueName = "invitation";
        private readonly IQueueService _queueService;

        public InvitationController(IQueueService queueService)
        {
            _queueService = queueService;
        }
        
        [HttpPost]
        public IActionResult Add([FromBody] InvitationDto invitation)
        {
            Console.WriteLine($"2_Service: Received an invitation request {JsonConvert.SerializeObject(invitation)}");
            _queueService.AddToQueue(InvitationQueueName, JsonConvert.SerializeObject(invitation));
           
            return Ok($"Invitation: {JsonConvert.SerializeObject(invitation)} received");
        }

    }
}
