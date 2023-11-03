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
            string result1 = HenkanJ.Instance.ToHalfsize(result);
            return Ok($"RomajiToKana: {result} - KanaToHalfsize{result1}");
        }
    }
}
