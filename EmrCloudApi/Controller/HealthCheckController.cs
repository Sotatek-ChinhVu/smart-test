using Helper.Common;
using Microsoft.AspNetCore.Http;
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
            string version = "20221115";
            return Ok("ConnectionString: " + connectionString + " Enviroment: " + enviroment + "Version: " + version);
        }
        
        
        [HttpGet("GetJapaneseCharacters")]
        public ActionResult<string> GetJapaneseCharacters()
        {
            string result = RomajiString.Instance.GetJapaneseCharacters();
            return Ok(result);
        }

        [HttpGet("ConvertRomajiToKana")]
        public ActionResult<string> ConvertRomajiToKana(string value)
        {
            string result = RomajiString.Instance.RomajiToKana(value);
            string result1 = HenkanJ.HankToZen(result);
            return Ok($"RomajiToKana: {result} - KanaToHalfsize{result1}");
        }
    }
}
