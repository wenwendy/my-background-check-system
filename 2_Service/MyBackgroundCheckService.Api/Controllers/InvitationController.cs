using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Api.DTOs;
using Newtonsoft.Json;
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
        public IActionResult Post([FromBody] InvitaitonDTO invitation)
        {
            Console.WriteLine($"received invitation: {invitation.Id}");
        
            _queueService.AddToQueue(InvitationQueueName, JsonConvert.SerializeObject(invitation));
           
            return Ok($"invitation: {invitation.Id} received");
        }

    }
}
