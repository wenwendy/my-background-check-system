﻿using System;
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
            Console.WriteLine($"Bob the provider received invitation: {JsonConvert.SerializeObject(invitation)}");
            
            return Ok();
        }

    }

    public class BobInvitationDto
    {
        public int Id { get; set; }
    }
}
