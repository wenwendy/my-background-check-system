using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Monolith.Api.DTOs;
using Newtonsoft.Json;

namespace Monolith.Api.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        private List<ResultDto> _results = new List<ResultDto>();

        [HttpGet]
        public IEnumerable<ResultDto> Get()
        {
            return _results;
        }

        
        [HttpPost]
        public void Post([FromBody] ResultDto result)
        {
            Console.WriteLine($"1_Monolith received result: {JsonConvert.SerializeObject(result)}");
            _results.Add(result);
        }

    }
}