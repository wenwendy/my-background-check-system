using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Monolith.Api.DTOs;
using Newtonsoft.Json;

namespace Monolith.Api.Controllers
{
    [Route("monolithapi/[controller]")]
    public class ResultController : Controller
    {
        private static List<ResultDto> _results = new List<ResultDto>();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(JsonConvert.SerializeObject(_results));
        }

        
        [HttpPost]
        public IActionResult Post([FromBody] ResultDto result)
        {
            Console.WriteLine($"1_Monolith received result: {JsonConvert.SerializeObject(result)}");
            _results.Add(result);

            return Ok();
        }

    }
}