using EmrCalculateApi.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutanController : ControllerBase
    {
        private readonly IFutanCalculate _futanCalculate;
        public FutanController(IFutanCalculate futanCalculate)
        {
            _futanCalculate = futanCalculate;
        }
    }
}
