using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.ViewModels;
using EmrCalculateApi.Requests;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecedenController : ControllerBase
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public RecedenController(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        [HttpPost("GetRecedenData")]
        public ActionResult GetRecedenData([FromBody] GetRecedenDataRequest input)
        {
            var recedenVm = new RecedenViewModel(_tenantProvider, _systemConfigProvider, _emrLogger);
            return Ok(recedenVm.GetRecedenData(input.Mode, input.Sort, input.HpId, input.SeikyuYM, input.OutputYM, input.SeikyuKbnMode, input.KaId, input.TantoId, input.IncludeTester, input.IncludeOutDrug));
        }
    }
}
