﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public HealthCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpGet("GetEnviroment")]
        public ActionResult<string> GetEnviroment()
        {
            string connectionString = _configuration["TenantDbSample"] ?? "Empty";
            string enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Empty";
            return Ok("ConnectionString: " + connectionString + " Enviroment " + enviroment);
        }
    }
}
