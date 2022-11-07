using EmrCalculateApi.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FutanController : ControllerBase
    {
        private readonly IFutancalcViewModel _futanCalculate;
        public FutanController(IFutancalcViewModel futanCalculate)
        {
            _futanCalculate = futanCalculate;
        }
    }
}
