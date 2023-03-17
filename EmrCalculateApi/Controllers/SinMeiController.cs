using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.ViewModels;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinMeiController : ControllerBase
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public SinMeiController(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        [HttpPost("GetSinMeiList")]
        public ActionResult<GetSinMeiListResponse> GetSinMeiList([FromBody] GetSinMeiListRequest request)
        {
            /*Mode = 3 Kaikei, 21 AccountingCard*/
            var sinMeiVM = request.SinMeiMode switch
            {
                3 => new SinMeiViewModel(SinMeiMode.Kaikei, false, request.HpId, request.PtId, request.SinDate, request.RaiinNoList, _tenantProvider, _systemConfigProvider, _emrLogger),
                21 => new SinMeiViewModel(SinMeiMode.AccountingCard, true, request.HpId, request.PtId, request.SinYm, request.HokenId, _tenantProvider, _systemConfigProvider, _emrLogger),
                _ => null
            };

            return new ActionResult<GetSinMeiListResponse>(new GetSinMeiListResponse(sinMeiVM?.SinMei ?? new()));

        }
    }
}
