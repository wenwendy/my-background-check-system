using System;
using Microsoft.AspNetCore.Mvc;
using MyBackgroundCheckService.Api.DTOs;
using Newtonsoft.Json;
using QueueService;

namespace MyBackgroundCheckService.Api.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        private const string ResultQueueName = "result";
        private readonly IQueueService _queueService;

        public ResultController(IQueueService queueService)
        {
            _queueService = queueService;
        }
        
        [HttpPost]
        public IActionResult Add([FromBody] ResultDto result)
        {
            Console.WriteLine($"received result: {JsonConvert.SerializeObject(result)}");
        
            _queueService.AddToQueue(ResultQueueName, JsonConvert.SerializeObject(result));
            
            return Ok();
        }
    }
}